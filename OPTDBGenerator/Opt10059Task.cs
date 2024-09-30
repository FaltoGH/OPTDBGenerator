using libKHOpenAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPTDBGenerator
{

    internal class Opt10059Task
    {
        private readonly string code;
        private readonly List<string> dts;

        public Opt10059Task(string jmcode, List<string> dates)
        {
            code = jmcode;
            dts = dates;
        }

        public void Run(KOAPI api)
        {
            foreach(string date in dts)
            {
                Opt10059Row[] rows = Opt10059.GetOpt10059RowsBuy(api, date, code);

            }
        }

    }

}
