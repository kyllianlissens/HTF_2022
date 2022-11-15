using System;
using System.Collections.Generic;

namespace HTF2022
{
   

    internal static class A3
    {
        private static readonly string testUrl = "/api/path/1/hard/Sample";
        private static readonly string productionUrl = "/api/path/1/hard/Puzzle";

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