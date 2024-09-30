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

        private static Opt10059Result GetOpt10059Rows(KOAPI api, string date, string jmcode, string tradeGubun)
        {
            api.SetInputValue("일자", date);
            api.SetInputValue("종목코드", jmcode);
            api.SetInputValue("금액수량구분", "2");
            api.SetInputValue("매매구분", tradeGubun);
            api.SetInputValue("단위구분", "1");

            Opt10059Row[] ret = null;
            AutoResetEvent are = new AutoResetEvent(false);
            RqResult ret2 = api.CommRqDataSync(api.NewRQName(), "OPT10059", 0, api.NewScrNo(), null, (o, e) =>
            {
                object commdataex = api.GetCommDataEx(e.sTrCode, e.sRecordName);
                object[,] commdataex2 = (object[,])commdataex;
                ret = Opt10059Row.FromDataEx2(commdataex2);
                are.Set();
            });
            are.WaitOne(0x3f3f3f3f);
            return new Opt10059Result(ret2, ret);
        }

        public static Opt10059Result GetOpt10059RowsBuy(KOAPI api, string date, string jmcode)
        {
            return GetOpt10059Rows(api, date, jmcode, "1");
        }

        public static Opt10059Result GetOpt10059RowsSell(KOAPI api, string date, string jmcode)
        {
            return GetOpt10059Rows(api, date, jmcode, "2");
        }

    }
}
