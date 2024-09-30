using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPTDBGenerator
{
    public static class SQLExtensions
    {

        public static List<string> GetAllTables(this SQLiteCommand cmd)
        {
            List<string> ret = new List<string>();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type=\"table\"";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ret.Add(reader.GetString(0));
            }
            reader.Close();
            return ret;
        }

    }
}
