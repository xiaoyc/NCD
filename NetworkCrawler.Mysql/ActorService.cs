using Dapper;
using Dapper.Contrib.Extensions;
using NetworkCrawler.Mysql.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql
{
    public class ActorService :Service
    {
        public long InsertActor(string name)
        {
            long actorId = 0;

            var actor = conn.Query<Actor>("select * from Actor where Name = @Name", new { Name = name }).AsList();

            if (actor.Count > 0)
            {
                actorId = actor[0].Id;
            }
            else
            {
                actorId = conn.Insert(new Actor() { Name = name });
            }

            return actorId;
        }
    }
}
