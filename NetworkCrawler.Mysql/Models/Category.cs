using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("Category")]
    public class Category
    {
        public long Id { get; set; }
        public string  Title { get; set; }
    }
}
