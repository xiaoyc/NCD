using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("PostTag")]
    public class PostTagMapping
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long TagId { get; set; }
    }
}
