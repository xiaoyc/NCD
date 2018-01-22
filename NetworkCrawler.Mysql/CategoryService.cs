using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using NetworkCrawler.Mysql.Models;
using Dapper.Contrib.Extensions;

namespace NetworkCrawler.Mysql
{
    public class CategoryService : Service
    {
        public long InsertCategory(string categoryName)
        {
            long categoryId = 0;

            var category = conn.Query<Category>("select * from Category where Title = @Title", new { Title = categoryName }).AsList();

            if (category.Count > 0)
            {
                categoryId = category[0].Id;
            }
            else
            {
                categoryId = conn.Insert(new Category() { Title = categoryName });
            }

            return categoryId;
        }
    }
}
