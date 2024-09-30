using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    public static class Opt10059
    {

        /// <returns>Key is PrevNext=="2". Value is Opt10059 rows.</returns>
        private static KeyValuePair<bool,Opt10059Row[]> __2024_0003(KOAPI api, string date, string jmcode, string tradeGubun)
        {
            api.SetInputValue("일자", date);
            api.SetInputValue("종목코드", jmcode);
            api.SetInputValue("금액수량구분", "2");
            api.SetInputValue("매매구분", tradeGubun);
            api.SetInputValue("단위구분", "1");

            Opt10059Row[] ret = null;
            AutoResetEvent are = new AutoResetEvent(false);
            KeyValuePair<int, bool> pair = api.CommRqDataSync(api.NewRQName(), "OPT10059", 0, api.NewScrNo(), null, (o, e) =>
            {
                object commdataex = api.GetCommDataEx(e.sTrCode, e.sRecordName);
                object[,] commdataex2 = (object[,])commdataex;
                ret = Opt10059Row.FromDataEx2(commdataex2);
                are.Set();
            });

            are.WaitOne(0x3f3f3f3f);

            if (pair.Key != 0) throw new Exception($"CommRqData returned {pair.Key}.");

            if (ret == null) throw new Exception("Return value is null!");

            return new KeyValuePair<bool, Opt10059Row[]>(pair.Value, ret);
        }

        /// <returns>Key is PrevNext=="2". Value is Opt10059 rows.</returns>
        private static KeyValuePair<bool, Opt10059Row[]> GetOpt10059RowsBuy(KOAPI api, string date, string jmcode)
        {
            return __2024_0003(api, date, jmcode, "1");
        }

        /// <returns>Key is PrevNext=="2". Value is Opt10059 rows.</returns>
        private static KeyValuePair<bool, Opt10059Row[]> GetOpt10059RowsSell(KOAPI api, string date, string jmcode)
        {
            return __2024_0003(api, date, jmcode, "2");
        }

        /// <returns>Key is PrevNext=="2". Value is Opt10059 pairs. If row count is zero, returns null.</returns>
        private static KeyValuePair<bool, KeyValuePair<Opt10059Row,Opt10059Row>[]>? GetOpt10059Pairs(KOAPI api, string date, string jmcode)
        {
            KeyValuePair<bool, Opt10059Row[]> buy = GetOpt10059RowsBuy(api, date, jmcode);
            if (buy.Value.Length <= 0)
            {
                return null;
            }
            KeyValuePair<bool, Opt10059Row[]> sell = GetOpt10059RowsBuy(api, date, jmcode);

            if (buy.Value.Length != sell.Value.Length)
            {
                throw new Exception($"Buy row count {buy.Value.Length} is not equal to sell row count {sell.Value.Length}");
            }

            KeyValuePair<Opt10059Row, Opt10059Row>[] ret = new KeyValuePair<Opt10059Row, Opt10059Row>[sell.Value.Length];

            for (int i = 0; i < sell.Value.Length; i++)
            {
                ret[i] = new KeyValuePair<Opt10059Row, Opt10059Row>(buy.Value[i], sell.Value[i]);
            }
            return new KeyValuePair<bool, KeyValuePair<Opt10059Row, Opt10059Row>[]>(sell.Key, ret);
        }

        /// <param name="requiredDates">Must be sorted by descending order before function call.</param>
        /// <returns>Descending order. Latest date is first. Oldest date is last.</returns>
        private static List<string> GetAllMissingDates(string[] requiredDates, HashSet<string> existingDates)
        {
            List<string> ret = new List<string>();
            int i = 0;
            while (i < requiredDates.Length)
            {
                bool? flag = existingDates?.Contains(requiredDates[i]);
                if (flag == true)
                {
                    i++;
                }
                else
                {
                    ret.Add(requiredDates[i]);
                    i += 100;
                }
            }
            return ret;
        }

        private static KeyValuePair<Opt10059Row, Opt10059Row>[] GetOpt10059PairsByMissingDates(
KOAPI api, string jmcode, List<string> missingDates)
        {
            GetOpt10059Pairs(api, )
        }

    }
}
