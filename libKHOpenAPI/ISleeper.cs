using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libKHOpenAPI
{

    public interface ISleeper:IDisposable
    {
        void BeginOrder();
        void EndOrder(int r);
        void BeginRq();
        void EndRq(int r);
    }

}
