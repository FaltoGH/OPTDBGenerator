﻿using libKHOpenAPI;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPTDBGenerator
{
    internal static class Opt10081Gen
    {
        public static void GenerateDataBase(KOAPI api)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=opt10081.db"))
            using (SQLiteCommand cmd = conn.CreateCommand())
            {
                conn.Open();
                List<string> tables = cmd.GetAllTables();
                HashSet<string> commonCodes = api.GetCommonCodes();
                string[] arrLast600 = Opt10081.Get600Dates(api);
                HashSet<string> last600set = arrLast600.ToHashSet();
                foreach (string table in tables)
                {
                    if (commonCodes.Contains(table))
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
                            Console.WriteLine($"Requirement already satisfied: {table}");
                        }
                    }
                }

                List<KeyValuePair<string, Opt10081Row[]>> db = new List<KeyValuePair<string, Opt10081Row[]>>();

                foreach (string jmcode in commonCodes.OrderBy(x=>x))
                {
                    if (db.Count > 1) break;
                    Opt10081Row[] rows = Opt10081.GetOpt10081Rows(api, jmcode);
                    db.Add(new KeyValuePair<string, Opt10081Row[]>(jmcode, rows));
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

                foreach(string table in tables)
                {
                    if (!commonCodes.Contains(table))
                    {
                        cmd.CommandText = $"DROP TABLE '{table}'";
                        cmd.ExecuteNonQuery();
                    }
                }

            }
        }
    }
}
