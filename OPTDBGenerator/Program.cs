using AxKHOpenAPILib;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using libKHOpenAPI;

namespace OPTDBGenerator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (KOAPI koapi = new KOAPI())
            {
                koapi.CommConnectSync();
                Opt10081Gen.GenerateDataBase(koapi);
                Opt10059Gen.GenerateDatabase(koapi);
            }
        }

    }

}
