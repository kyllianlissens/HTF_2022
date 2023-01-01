using System;
using System.IO;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HTF2022
{
    public class Grid
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string content { get; set; }
    }

    public class Caesar
    {
        public List<string> cipheredWords { get; set; }
        public List<Grid> grid { get; set; }
    }

    internal static class B3
    {
        private static readonly string testUrl = "/api/path/2/hard/Sample";
        private static readonly string productionUrl = "/api/path/2/hard/Puzzle";

        private static readonly HTTPInstance clientInstance = new();

        internal static void LocalExecution()
        {
            Console.WriteLine("-Local Execution: \n");
        }

        internal static async Task TestExecution()
        {
            Console.WriteLine("-Test Execution: \n");
            var testData = await clientInstance.Client.GetFromJsonAsync<Caesar>(testUrl);
            Console.WriteLine($"Test endpoint data: {string.Join("; ", testData)}");
            var decryptedWords = testData.cipheredWords.Select(x => Decrypt(x, 10));
            var ids = new List<int>();
            foreach (var decryptedWord in decryptedWords)
            {
                ids.AddRange(FindWords(testData.grid, decryptedWord));
            }

            var testSolution = testData.grid.Where(x => !ids.Contains(x.id)).OrderBy(x => x.id).Select(x => x.content)
                .ToArray();

            var testPostResponse = await clientInstance.Client.PostAsJsonAsync<string[]>(testUrl, testSolution);
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
            var json = File.ReadAllText($"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent}/B3prod.json");
            var productionData = JsonSerializer.Deserialize<Caesar>(json);
            //var productionData = await clientInstance.Client.GetFromJsonAsync<Caesar>(productionUrl);


            var decryptedWords = productionData.cipheredWords.Select(x => Decrypt(x, 14));


            var ids = new List<int>();
            productionData.grid = productionData.grid.OrderBy(x => x.y).ThenBy(x => x.x).ToList();

            var d = DecryptGrid(productionData.grid, 6);

            foreach (var decryptedWord in decryptedWords)
            {
                ids.AddRange(FindWords(d, decryptedWord));
            }
            
            var productionSolution = productionData.grid.Where(x => !ids.Contains(x.id)).OrderBy(x => x.id)
                .Select(x => x.content).ToArray();
            Console.WriteLine(string.Join("", productionSolution)); // res itchitacupitamelakamystica -> itchita copita melaka mystica

            //var productionPostResponse = await clientInstance.Client.PostAsJsonAsync<string[]>(productionUrl, productionSolution);
            //var productionPostResponseValue = await productionPostResponse.Content.ReadAsStringAsync();
            //Console.WriteLine($"Production endpoint response: {productionPostResponseValue}");
        }


        static List<Grid> DecryptGrid(List<Grid> grid, int key)
        {
            var newList = new List<Grid>();

            foreach (var VARIABLE in grid)
            {
                var newGrid = new Grid
                {
                    id = VARIABLE.id,
                    x = VARIABLE.x,
                    y = VARIABLE.y,
                    content = Decrypt(VARIABLE.content, key)
                };

                newList.Add(newGrid);
            }

            return newList;
        }
        
        
        private static bool Move(Grid coordinate, IReadOnlyCollection<Grid> matrix, string word, int i, int j)
        {
            if (word == coordinate.content && word.Length == 1) return true;
            var (maxX, maxY) = (matrix.Max(x => x.x), matrix.Max(x => x.y));
            
            if (i == -1 && (coordinate.x <= 1 || coordinate.y >= maxY)) return false;
            if (i == 1 && j == 1 && (coordinate.x >= maxX || coordinate.y >= maxY)) return false;
            if (i == 1 && j == 0 && coordinate.x >= maxX) return false;
            if (i == 0 && j == 1 && coordinate.y >= maxY) return false;

            return word[0] == coordinate.content[0] &&
                   Move(matrix.First(x => x.x == coordinate.x + i && x.y == coordinate.y + j), matrix, word[1..], i, j);
        }

        private static List<int> FindWords(List<Grid> grid, string word, bool reversed = false)
        {
            var ids = new List<int>();

            var starts = grid.Where(x => x.content[0] == word[0]).ToArray();
            foreach (var coordinate in starts)
            {
                
                if (Move(coordinate, grid, word,1,0)) // left right
                    return GetIds(coordinate, word, 1, 0, grid);
                
                if (Move(coordinate, grid, word, 0, 1)) // up down
                    return GetIds(coordinate, word, 0, 1, grid);

                if (Move(coordinate, grid, word, 1, 1)) //top left, bottom right
                    return GetIds(coordinate, word, 1, 1, grid);

                if (Move(coordinate, grid, word, -1, 1)) // top right, bottom left
                    return GetIds(coordinate, word, -1, 1, grid);

                if (!reversed && coordinate.id == starts.Last().id)
                    return FindWords(grid, Reverse(word), true);
            }


            return ids;
        }

        static List<int> GetIds(Grid start, string word, int cx, int cy, List<Grid> matrix)
        {
            var ids = new List<int>();
            ids.Add(start.id);
            for (int i = 1; i < word.Length; i++)
            {
                ids.Add(matrix.First(x => x.x == start.x + (i * cx) && x.y == start.y + (i * cy)).id);
            }

            return ids;
        }

        private static string Reverse(string s)
        {
            return s.Aggregate(string.Empty, (current, c) => c + current);
        }

        private static string Decrypt(String cipherText, int shiftKey)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            var message = "";
            foreach (var t in cipherText)
            {
                var charPosition = alphabet.IndexOf(t);
                var keyVal = (charPosition - shiftKey) % 26;
                if (keyVal < 0)
                    keyVal = 26 + keyVal;
                
                var replaceVal = alphabet[keyVal];
                message += replaceVal;
            }
            return message;
        }
    }
}