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
            using (KOAPI koapi = new KOAPI())
            {
                koapi.CommConnectSync();
                string[] commonCodes = koapi.GetCommonCodes();
                List<KeyValuePair<string, Opt10081Row[]>> db = new List<KeyValuePair<string, Opt10081Row[]>>();

                byte i = 0;
                foreach (string code in commonCodes)
                {
                    if (i >= 5) break;
                    Opt10081Row[] rows = koapi.GetOpt10081Rows(code);
                    db.Add(new KeyValuePair<string, Opt10081Row[]>(code, rows));
                    i++;
                }

                using (SQLiteConnection conn = new SQLiteConnection("Data Source=opt10081.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = conn.CreateCommand())
                    {
                        foreach (KeyValuePair<string, Opt10081Row[]> x in db)
                        {
                            string jmcode = x.Key;
                            cmd.CommandText = $@"CREATE TABLE IF NOT EXISTS '{jmcode}'(현재가 INTEGER,
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
}