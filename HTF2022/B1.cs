using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HTF2022
{

    internal static class B1
    {

        private static readonly string testUrl = "/api/path/2/easy/Sample";
        private static readonly string productionUrl = "/api/path/2/easy/Puzzle";

        private static readonly HTTPInstance clientInstance = new HTTPInstance();

        internal static void LocalExecution()
        {
            var list = new List<string>(){"test","string"};
            var chars = GetCharacterAndCount(list);
            Console.WriteLine(GetWord(chars));
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

        static string? GetWord(List<IGrouping<char, char>> chars)
        {
            var word = new char[chars.Max(x => x.Count())];
            foreach (var character in chars)
            {
                word[character.Count() - 1] = character.Key;
            }
            return new string(word);
        }

        static List<IGrouping<char, char>> GetCharacterAndCount(IEnumerable<string> strings)
        {
            return strings.SelectMany(VARIABLE => VARIABLE.ToCharArray().GroupBy(x => x)).ToList();
        }
    }
}
