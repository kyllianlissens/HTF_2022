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
            Console.WriteLine("-Local Execution: \n");
            var list = new List<string>(){ "cosuuscsshocsuu", "ucsssscucuoopsu", "assssapttt"};
            var chars = GetCharacterAndCount(list);
            var testSolution = string.Join(" ", chars);
            Console.WriteLine(testSolution);

        }

        internal async static Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
            var testData = await clientInstance.Client.GetFromJsonAsync<List<string>>(testUrl);
            Console.WriteLine($"Test endpoint data: {string.Join("; ", testData)}");
            var chars = GetCharacterAndCount(testData);
            var testSolution = string.Join(" ",chars);
            Console.WriteLine(testSolution);
            var testPostResponse = await clientInstance.Client.PostAsJsonAsync<string>(testUrl, testSolution);
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Test endpoint response: {testPostResponseValue}");

        }

        internal async static Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
            var productionData = await clientInstance.Client.GetFromJsonAsync<List<string>>(productionUrl);
            Console.WriteLine($"Test endpoint data: {string.Join("; ", productionData)}");
            var chars = GetCharacterAndCount(productionData);
            var productionSolution = string.Join(" ", chars);
            Console.WriteLine(productionSolution);
            var productionPostResponse = await clientInstance.Client.PostAsJsonAsync<string>(productionUrl, productionSolution);
            var productionPostResponseValue = await productionPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Production endpoint response: {productionPostResponseValue}");
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

        static List<string?> GetCharacterAndCount(IEnumerable<string> strings)
        {
            return (from str in strings select str.ToCharArray().GroupBy(x => x) 
                into perChar select perChar.ToList() 
                into chars 
                select GetWord(chars)).ToList();
        }
    }
}
