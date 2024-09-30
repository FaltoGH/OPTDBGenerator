using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    public class Opt10059Row
    {
        public string Date;
        public long Close;
        public long Symbol;
        public long Diff;
        public string Rate;
        public long Volume;
        public long Money;
        public long Person;
        public long Foreigner;
        public long Org;
        public long Finance;
        public long Insurance;
        public long Invest;
        public long MiscFinance;
        public long Bank;
        public long Pension;
        public long PEF;
        public long Nation;
        public long MiscCorp;
        public long LocalForeigner;

        public static Opt10059Row FromDataEx(object[,] dataex, int i)
        {
            string ToString(int x)
            {
                string s = dataex[i, x].ToString();
                if (string.IsNullOrWhiteSpace(s))
                {
                    s = "0";
                }
                return s;
            }
            long ToLong(int x)
            {
                long.TryParse(dataex[i, x].ToString(), out long result);
                return result;
            }
            Opt10059Row ret = new Opt10059Row();
            ret.Date = ToString(0);
            ret.Close = ToLong(1);
            ret.Symbol = ToLong(2);
            ret.Diff = ToLong(3);
            ret.Rate = ToString(4);
            ret.Volume = ToLong(5);
            ret.Money = ToLong(6);
            ret.Person = ToLong(7);
            ret.Foreigner = ToLong(8);
            ret.Org = ToLong(9);
            ret.Finance = ToLong(10);
            ret.Insurance = ToLong(11);
            ret.Invest = ToLong(12);
            ret.MiscFinance = ToLong(13);
            ret.Bank = ToLong(14);
            ret.Pension = ToLong(15);
            ret.PEF = ToLong(16);
            ret.Nation = ToLong(17);
            ret.MiscCorp = ToLong(18);
            ret.LocalForeigner = ToLong(19);
            return ret;
        }

        public static Opt10059Row[] FromDataEx2(object[,] dataex)
        {
            int nrow = dataex.GetLength(0);
            Opt10059Row[] ret = new Opt10059Row[nrow];
            for (int i = 0; i < nrow; i++)
                ret[i] = Opt10059Row.FromDataEx(dataex, i);
            return ret;
        }
    }
}
