using System;

namespace HTF2022
{
    internal static class B3
    {
        private static readonly string testUrl = "";
        private static readonly string productionUrl = "";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
        }

        internal static void TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
        }

        internal static void ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
        }
    }
}