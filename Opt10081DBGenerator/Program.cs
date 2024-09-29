using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Opt10081DBGenerator
{
    internal static class Program
    {
        public static string DirectoryName;
        private static void Main(string[] args)
        {
            DirectoryName = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            KOAPI koapi = new KOAPI();
            koapi.CommConnectSync();
            HashSet<string> commonCodes = koapi.GetCommonCodes();
            foreach(string code in commonCodes)
            {
                koapi.GetOpt10081Rows(code);
            }
        }
    }
}