using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WashinqV2.Database
{
    class Database
    {
        private static string connection = "server=localhost; uid=root; pwd=; database=washinq";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connection);
        }
    }
}
