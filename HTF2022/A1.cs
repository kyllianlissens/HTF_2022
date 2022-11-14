using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTF2022
{
    internal static class A1
    {
        private static readonly string testUrl = "";
        private static readonly string productionUrl = "";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");

        }

        internal static async Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
           
        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
            
        }

    }
}