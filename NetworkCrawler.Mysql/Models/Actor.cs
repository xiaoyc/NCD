using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("Actor")]
    public class Actor
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
