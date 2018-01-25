using DotnetSpider.Core;
using DotnetSpider.Core.Downloader;
using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Scheduler;
using DotnetSpider.Core.Selector;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Downloader;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NetworkCrawlerManager.Core
{
    public class JiraSpider
    {
        //protected override void MyInit(params string[] arguments)
        //{
        //    Identity = $"Jira test {DateTime.Now.ToString("yyyy-MM-dd HHmmss")}";
        //    AddStartUrl("https://jira-fe-p01.mr.ericsson.se:8443/browse/MF-86514?filter=-4");
        //}

        //[TargetUrlsSelector(XPaths = new[] { "//div[@class=\"pagination\"]/a[@herf]" })]
        //class Product : SpiderEntity
        //{
        //    string Title { get; set; }
        //}


        public static void Run()
        {
            // 定义要采集的 Site 对象, 可以设置 Header、Cookie、代理等
            var site = new Site { EncodingName = "UTF-8" };

            // 添加初始采集链接
            site.AddStartUrl("https://jira-fe-p01.mr.ericsson.se:8443/browse/MF-86514?filter=-4");


            // 使用内存Scheduler、自定义PageProcessor、自定义Pipeline创建爬虫
            Spider spider = Spider.Create(site,
                new QueueDuplicateRemovedScheduler(),
                new YoukuPageProcessor())
                .AddPipeline(new MyPipeline());


           // string[] domains = "Cookie:jira.editor.user.mode=wysiwyg; JSESSIONID=EAA5BCB1E577D3B3C29F489AE6395BD8; seraph.rememberme.cookie=25739%3A42a854f54d64f2e63c8b464e81412b76eba3976c; atlassian.xsrf.token=BEYL-3U3I-DALF-V315|6629f56380b6bcd9ef2956c7284931797429555a|lin".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

           
            spider.Downloader = new WebDriverDownloader(Browser.Chrome);
            spider.ThreadNum = 1;

            // 启动爬虫
            spider.Run();
        }
    }

    class JiaCookieInjector : CookieInjector
    {
        protected override CookieCollection GetCookies(ISpider spider)
        {
            throw new NotImplementedException();
        }
    }
    public class YoukuPageProcessor : BasePageProcessor
    {
        protected override void Handle(Page page)
        {
            var totalElements = page.Selectable.SelectList(Selectors.XPath("//span[@class='issue-link-summary']/text()")).Nodes();

            List<WorkItem> results = new List<WorkItem>();
            foreach (var ele in totalElements)
            {
                var workItem = new WorkItem();
                workItem.Title = ele.GetValue();
                results.Add(workItem);
            }

            // Save data object by key. 以自定义KEY存入page对象中供Pipeline调用
            page.AddResultItem("Result", results);


            foreach (var url in page.Selectable.SelectList(Selectors.XPath("//div[@class='pagination']")).Links().Nodes())
            {
                page.AddTargetRequest(new Request(url.GetValue(), null));
            }
        }
    }

    class WorkItem
    {
        public string Title { get; set; }
    }

    class MyPipeline : BasePipeline
    {
        private static long count = 0;
        public override void Process(IEnumerable<ResultItems> resultItems, ISpider spider)
        {
            foreach (var resultItem in resultItems)
            {
                StringBuilder builder = new StringBuilder();
                foreach (WorkItem entry in resultItem.Results["Result"])
                {
                    count++;
                    builder.Append($" [YoukuVideo {count}] {entry.Title}");
                }
                Console.WriteLine(builder);
            }
        }
    }
}