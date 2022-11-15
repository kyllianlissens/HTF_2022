using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTF2022
{
    internal static class A1
    {

        private static Dictionary<char, int> RomanMap = new Dictionary<char, int>()
        {
            {'I', 1},
            {'V', 5},
            {'X', 10},
            {'L', 50},
            {'C', 100},
            {'D', 500},
            {'M', 1000}
        };


        private static readonly string testUrl = "/api/path/1/easy/Sample";
        private static readonly string productionUrl = "/api/path/1/easy/Puzzle";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
            //test RomanToNumber
            Console.WriteLine("RomanToNumber(\"IL\") = " + RomanToNumber("IL"));
            Console.WriteLine("RomanToNumber(\"XLVIII\") = " + RomanToNumber("XLVIII"));

        }

        internal static async Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");

        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");

        }

        internal static int RomanToNumber(string input)
        {
            int number = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (i + 1 < input.Length && RomanMap[input[i]] < RomanMap[input[i + 1]])
                {
                    number -= RomanMap[input[i]];
                }
                else
                {
                    number += RomanMap[input[i]];
                }
            }
            return number;

        }
    }
}