using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace libKHOpenAPI
{
    internal sealed class SafeSleeper : IDisposable, ISleeper
    {
        private readonly Stopwatch m_orderStopwatch = Stopwatch.StartNew();
        private readonly Stopwatch m_rqStopwatch = Stopwatch.StartNew();
        private bool IsDisposed;

        public SafeSleeper()
        {
            Thread t = new Thread(f);
            t.IsBackground = true;
            t.Name = "sleeper";
            t.Start();
        }

        private int m_stack;
        private void f()
        {
            while (!IsDisposed)
            {
                Thread.Sleep(190000);//3m 10s
                Interlocked.Exchange(ref m_stack, 0);
            }
        }

        private readonly TimeSpan ms200 = TimeSpan.FromMilliseconds(200);
        private void Sleep(TimeSpan x)
        {
            if (x > TimeSpan.Zero)
            {
                Thread.Sleep(x);
            }
        }

        public void BeginOrder()
        {
            TimeSpan ts = ms200 - m_orderStopwatch.Elapsed;
            Sleep(ts);
        }

        public void EndOrder(int ret)
        {
            if (ret == 0)
            {
                m_orderStopwatch.Restart();
            }
        }

        public void BeginRq()
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(200 + m_stack * 147) - m_rqStopwatch.Elapsed;
            Sleep(ts);
        }

        public void EndRq(int ret)
        {
            if (ret == 0)
            {
                m_rqStopwatch.Restart();
                Interlocked.Increment(ref m_stack);
            }
        }

        public void Dispose()
        {
            IsDisposed = true;
        }

    }
}
