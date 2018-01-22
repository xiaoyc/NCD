using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using NetworkCrawler.Mysql.Models;
using Dapper.Contrib.Extensions;

namespace NetworkCrawler.Mysql
{
    public class TagService : Service
    {
        public long InsertTag(string tagName)
        {
            long tagId = 0;

            var tag = conn.Query<Tag>("select * from tag where Title = @Title", new { Title = tagName }).AsList();

            if (tag.Count > 0)
            {
                tagId = tag[0].Id;
            }
            else
            {
                tagId = conn.Insert(new Tag() { Title = tagName });
            }

            return tagId;
        }
    }
}
