using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("Tag")]
    public class Tag
    {
        public long Id { get; set; }
        public string  Title { get; set; }
    }
}
