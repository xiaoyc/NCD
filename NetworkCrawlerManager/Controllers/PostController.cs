using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetworkCrawler.Mysql;
using NetworkCrawler.Mysql.Models;
using Newtonsoft.Json;

namespace NetworkCrawlerManager.Controllers
{
    [Produces("application/json")]
    [Route("api/Post")]
    public class PostController : Controller
    {
        PostService postService = new PostService();
        // GET: api/Post
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Post/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        
        // POST: api/Post
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var post = JsonConvert.DeserializeObject<Post>(value);
            postService.InsertPost(post, false);

        }
        
        // PUT: api/Post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
