using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTF2022
{
    internal static class B2
    {
        private static readonly string testUrl = "/api/path/2/medium/Sample";
        private static readonly string productionUrl = "/api/path/2/medium/Puzzle";

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