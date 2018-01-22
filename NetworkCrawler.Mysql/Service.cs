using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCrawler.Mysql
{
    public class Service
    {
       public MySqlConnection conn = new MySqlConnection("server=127.0.0.1;database=resource;uid=root;pwd=root;charset='gbk'");
    }
}
