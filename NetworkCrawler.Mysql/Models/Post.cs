using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string VideoUrl { get; set; }
        public byte[] Image { get; set; }
        public string OriginalPageUrl { get; set; }
        public int OriginalPageId { get; set; }
        public long CategoryId { get; set; }

        [Write(false)]
        public string CategoryName { get; set; }

        [Write(false)]
        public string Tags { get; set; }
        [Write(false)]
        public string Actors { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
