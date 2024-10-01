using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using libKHOpenAPI;
using System.Threading.Tasks;

namespace OPTDBGenerator
{

    internal static class Opt10059Gen
    {
        private static HashSet<string> GetAllDates(SQLiteCommand cmd, string table)
        {
            HashSet<string> ret = new HashSet<string>();
            cmd.CommandText = $"SELECT 일자 FROM '{table}'";
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ret.Add(reader.GetString(0));
            }
            reader.Close();
            return ret;
        }



        public static void GenerateDatabase(KOAPI api)
        {
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=opt10059.db"))
            {
                conn.Open();
                using (SQLiteCommand cmd = conn.CreateCommand())
                {
                    //
                    // Drop unnecessary tables
                    //
                    HashSet<string> allTables = cmd.GetAllTables().ToHashSet();
                    HashSet<string> setCommonCodes = api.GetCommonCodes();
                    foreach(string table in allTables.Except(setCommonCodes))
                    {
                        cmd.CommandText = $"DROP TABLE '{table}'";
                        cmd.ExecuteNonQuery();
                    }
                    allTables.ExceptWith(setCommonCodes);
                    if (!allTables.IsSubsetOf(setCommonCodes))
                    {
                        throw new Exception("Table set is not a subset of common codes.");
                    }
                    //
                    // Request
                    //
                    string[] date600 = Opt10081.Get600Dates(api);
                    string lastDate = date600.Last();
                    List<KeyValuePair<string, List<KeyValuePair<Opt10059Row, Opt10059Row>>>> listDB;
                    listDB = new List<KeyValuePair<string, List<KeyValuePair<Opt10059Row, Opt10059Row>>>>();
                    foreach(string commonCode in setCommonCodes.OrderBy(x=>x))
                    {
                        if (listDB.Count > 0) break;
                        HashSet<string> existingDates;
                        if (allTables.Contains(commonCode))
                        {
                            existingDates = GetAllDates(cmd, commonCode);
                        }
                        else
                        {
                            existingDates = null;
                        }

                        List<KeyValuePair<Opt10059Row, Opt10059Row>> pairs = Opt10059.GetOpt10059PairsWithExistingDates(
api, commonCode, date600, existingDates);

                        if(pairs == null)
                        {
                            Console.WriteLine($"Requirement already satisfied: {commonCode}");
                        }
                        else
                        {
                            listDB.Add(new KeyValuePair<string, List<KeyValuePair<Opt10059Row, Opt10059Row>>>(commonCode, pairs));
                        }
                    }

                    foreach (KeyValuePair<string, List<KeyValuePair<Opt10059Row, Opt10059Row>>> row in listDB)
                    {
                        string jmcode = row.Key;
                        cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS '{jmcode}'(일자 TEXT PRIMARY KEY,
현재가 INTEGER,
대비기호 INTEGER,
전일대비 INTEGER,
등락율 TEXT,
누적거래량 INTEGER,
누적거래대금 INTEGER,
개인투자자B INTEGER,
외국인투자자B INTEGER,
기관계B INTEGER,
금융투자B INTEGER,
보험B INTEGER,
투신B INTEGER,
기타금융B INTEGER,
은행B INTEGER,
연기금등B INTEGER,
사모펀드B INTEGER,
국가B INTEGER,
기타법인B INTEGER,
내외국인B INTEGER,
개인투자자S INTEGER,
외국인투자자S INTEGER,
기관계S INTEGER,
금융투자S INTEGER,
보험S INTEGER,
투신S INTEGER,
기타금융S INTEGER,
은행S INTEGER,
연기금등S INTEGER,
사모펀드S INTEGER,
국가S INTEGER,
기타법인S INTEGER,
내외국인S INTEGER)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = $"INSERT OR IGNORE INTO '{jmcode}' VALUES " + Opt10059Pair.ToValues(
row.Value.ConvertAll(x=>new Opt10059Pair(x)));
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                            Console.WriteLine(cmd.CommandText);
                        }
                        cmd.CommandText = $"DELETE FROM '{jmcode}' WHERE 일자 < '{lastDate}'";
                        cmd.ExecuteNonQuery();
                    }

                }

            }

        }

    }

}
