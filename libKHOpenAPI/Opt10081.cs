using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace libKHOpenAPI
{

    public static class Opt10081
    {

        public static Opt10081Row[] GetOpt10081Rows(KOAPI api, string jmcode)
        {
            api.SetInputValue("종목코드", jmcode);
            Opt10081Row[] ret = null;
            AutoResetEvent are = new AutoResetEvent(false);
            api.CommRqDataSync(api.NewRQName(), "OPT10081", 0, api.NewScrNo(), null, (o, e) =>
            {
                object commdataex = api.GetCommDataEx(e.sTrCode, e.sRecordName);
                object[,] commdataex2 = (object[,])commdataex;
                ret = Opt10081Row.FromDataEx2(commdataex2);
                are.Set();
            });
            are.WaitOne(0x3f3f3f3f);
            return ret;
        }

        private static string[] cache;
        /// <returns>Descending order. Latest date is first. Oldest date is last.</returns>
        public static string[] Get600Dates(KOAPI api)
        {
            if(cache != null)
            {
                return cache;
            }
            string[] ret = GetOpt10081Rows(api, "005930").Select(x => x.Date).ToArray();
            if (ret.Length != 600)
            {
                throw new Exception($"Expected return array length 600, but actual is {ret.Length}.");
            }
            cache = ret;
            return ret;
        }

    }

}
