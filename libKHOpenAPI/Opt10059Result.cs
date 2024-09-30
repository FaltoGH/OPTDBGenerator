using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    public class Opt10059Result
    {
        public RqResult RequestResult;
        public Opt10059Row[] Opt10059Rows;

        public Opt10059Result(RqResult rqResult, Opt10059Row[] rows)
        {
            RequestResult = rqResult;
            Opt10059Rows = rows;
        }

    }
}
