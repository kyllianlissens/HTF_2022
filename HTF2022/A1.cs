using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTF2022
{
    internal static class A1
    {
        private static readonly string testUrl = "api/path/1/easy/Sample";
        private static readonly string productionUrl = "api/path/1/easy/Puzzle";

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