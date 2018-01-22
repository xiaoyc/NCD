using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql.Models
{
    [Table("PostActor")]
    public class PostActorMapping
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public long ActorId { get; set; }
    }
}
