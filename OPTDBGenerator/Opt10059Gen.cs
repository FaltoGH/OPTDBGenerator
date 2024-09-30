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
                    HashSet<string> allTables = cmd.GetAllTables().ToHashSet();
                    HashSet<string> setCommonCodes = api.GetCommonCodes();

                    foreach(string table in allTables.Except(setCommonCodes))
                    {
                        cmd.CommandText = $"DROP TABLE '{table}'";
                        cmd.ExecuteNonQuery();
                    }

                    allTables.ExceptWith(setCommonCodes);

                    string[] date600 = Opt10081.Get600Dates(api);

                    List<Opt10059Task> tasks = new List<Opt10059Task>();

                    if (!allTables.IsSubsetOf(setCommonCodes))
                    {
                        throw new Exception("Table set is not a subset of common codes.");
                    }

                    foreach(string commonCode in setCommonCodes)
                    {
                        HashSet<string> existingDates;
                        if (allTables.Contains(commonCode))
                        {
                            existingDates = GetAllDates(cmd, commonCode);
                        }
                        else
                        {
                            existingDates = null;
                        }
                        List<string> allMissingDates = GetAllMissingDates(date600, existingDates);
                        if (allMissingDates.Count > 0)
                        {
                            tasks.Add(new Opt10059Task(commonCode, allMissingDates));
                        }
                    }

                    List<KeyValuePair<string, Opt10059Pair[]>> listDB = new List<KeyValuePair<string, Opt10059Pair[]>>();

                    byte i = 0;
                    foreach (Opt10059Task task in tasks)
                    {
                        if (i >= 4) break;
                        

                        foreach (string date in task.Value)
                        {
                            Opt10059Row[] rows = Opt10059.GetOpt10059RowsBuy(api, task.Key);
                            listDB.Add(new KeyValuePair<string, Opt10081Row[]>(jmcode, rows));
                        }

                        i++;
                    }

                    foreach (KeyValuePair<string, Opt10081Row[]> x in listDB)
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

}
