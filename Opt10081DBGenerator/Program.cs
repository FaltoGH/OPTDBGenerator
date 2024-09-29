using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Opt10081DBGenerator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=opt10081.db"))
            using (SQLiteCommand cmd = conn.CreateCommand())
            using (KOAPI koapi = new KOAPI())
            {
                conn.Open();
                List<string> tables = cmd.GetAllTables();
                koapi.CommConnectSync();
                HashSet<string> commonCodes = koapi.GetCommonCodes();
                string[] arrLast600 = koapi.GetLast600MarketOpenDates();
                HashSet<string> last600set = arrLast600.ToHashSet();
                foreach (string table in tables)
                {
                    HashSet<string> dates = new HashSet<string>();
                    cmd.CommandText = $"SELECT 일자 FROM '{table}'";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        dates.Add(reader.GetString(0));
                    }
                    reader.Close();
                    if (dates.SetEquals(last600set))
                    {
                        commonCodes.Remove(table);
                        Console.WriteLine($"Skipped {table}");
                    }
                }

                List<KeyValuePair<string, Opt10081Row[]>> db = new List<KeyValuePair<string, Opt10081Row[]>>();

                byte i = 0;
                foreach (string jmcode in commonCodes)
                {
                    if (i >= 4) break;
                    Opt10081Row[] rows = koapi.GetOpt10081Rows(jmcode);
                    db.Add(new KeyValuePair<string, Opt10081Row[]>(jmcode, rows));
                    i++;
                }

                foreach (KeyValuePair<string, Opt10081Row[]> x in db)
                {
                    string jmcode = x.Key;
                    cmd.CommandText = $"DROP TABLE IF EXISTS '{jmcode}'";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $@"CREATE TABLE '{jmcode}'(현재가 INTEGER,
거래량 INTEGER,
거래대금 INTEGER,
일자 TEXT PRIMARY KEY,
시가 INTEGER,
고가 INTEGER,
저가 INTEGER,
수정주가구분 INTEGER,
수정비율 TEXT)";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = $"INSERT INTO '{jmcode}' VALUES " + Opt10081Row.ToValues(x.Value);
                    cmd.ExecuteNonQuery();
                }

            }

        }

    }

}
