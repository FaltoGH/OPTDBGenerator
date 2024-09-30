using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    public class Opt10081Row
    {
        public long Close;
        public long Volume;
        public long VolumeMoney;
        public string Date;
        public long Open;
        public long High;
        public long Low;
        public long ModifyType;
        public string ModifyRatio;

        public static Opt10081Row FromDataEx(object[,] dataex, int i)
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
            Opt10081Row ret = new Opt10081Row();
            ret.Close = ToLong(1);
            ret.Volume = ToLong(2);
            ret.VolumeMoney = ToLong(3);
            ret.Date = ToString(4);
            ret.Open = ToLong(5);
            ret.High = ToLong(6);
            ret.Low = ToLong(7);
            ret.ModifyType = ToLong(8);
            ret.ModifyRatio = ToString(9);
            return ret;
        }

        public static Opt10081Row[] FromDataEx2(object[,] dataex)
        {
            int nrow = dataex.GetLength(0);
            Opt10081Row[] ret = new Opt10081Row[nrow];
            for (int i = 0; i < nrow; i++)
                ret[i] = FromDataEx(dataex, i);
            return ret;
        }

        public string ToValue()
        {
            return $"({Close},{Volume},{VolumeMoney},'{Date}',{Open},{High},{Low},{ModifyType},'{ModifyRatio}')";
        }

        public static string ToValues(Opt10081Row[] rows)
        {
            return string.Join(", ", rows.Select(x => x.ToValue()));
        }

    }
}
