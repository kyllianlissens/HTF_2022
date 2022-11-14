using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HTF2022
{

    internal static class B1
    {

        private static readonly string testUrl = "";
        private static readonly string productionUrl = "";

        private static readonly HTTPInstance clientInstance = new HTTPInstance();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
           
        }

        internal async static Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
            
        }

        internal async static Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
           
        }
    }
}
