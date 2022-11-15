using System;
using System.Net.Http.Json;
using System.Security.Cryptography;

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
            var testSolution = testData.grid.Where(x => !ids.Contains(x.id)).OrderBy(x => x.id).Select(x => x.content).ToArray();

            var testPostResponse = await clientInstance.Client.PostAsJsonAsync<string[]>(testUrl, testSolution);
            var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
        }

        internal static async Task ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
            Console.WriteLine("-Production Execution: \n");
            
            var productionData = await clientInstance.Client.GetFromJsonAsync<Caesar>(productionUrl);
            Console.WriteLine($"Test endpoint data: {string.Join("; ", productionData.cipheredWords)}");
            var decryptedWords = productionData.cipheredWords.Select(x => Decrypt(x, 14));

            
            var decryptedGrid = DecyptGrid(productionData.grid, 4);
            var ids = new List<List<int>>();
            foreach (var decryptedWord in decryptedWords)
            {
                ids.Add(FindWords(decryptedGrid, decryptedWord));
            }

            Console.WriteLine(ids.Count(x => x != null));
            //for (int i = 0; i < 26; i++)
            //{
            //    Console.WriteLine(i);
            //    var decryptedGrid = DecyptGrid(productionData.grid, i);
            //    var ids = new List<List<int>>();
            //    foreach (var decryptedWord in decryptedWords)
            //    {
            //        ids.Add(FindWords(decryptedGrid, decryptedWord));
            //    }

            //    Console.WriteLine(ids.Count(x => x!= null));
            //    //printGrid(decryptedGrid);

            //}



            //Console.WriteLine(productionSolution);
            //var productionPostResponse = await clientInstance.Client.PostAsJsonAsync<string>(productionUrl, productionSolution);
            //var productionPostResponseValue = await productionPostResponse.Content.ReadAsStringAsync();
            //Console.WriteLine($"Production endpoint response: {productionPostResponseValue}");
        }

        static void printGrid(List<Grid> grid)
        {
            var z = new string[grid.Max(x => x.x), grid.Max(x => x.x)];
            foreach (var VARIABLE in grid)
            {
                z[VARIABLE.x - 1, VARIABLE.y - 1] = VARIABLE.content;
            }

            for (int i = 0; i < z.GetLength(0); i++)
            {
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    Console.Write(z[i,j]+" ");
                }

                Console.WriteLine();
            }
        }
        static List<Grid> DecyptGrid(List<Grid> grid, int key)
        {
            foreach (var VARIABLE in grid)
            {
                VARIABLE.content = Decrypt(VARIABLE.content, key);
            }

            return grid;
        }

        static (bool, Grid) MoveDiagonalRight(Grid coordinate, List<Grid> matrix, string word)
        {
            if (word[^1] == coordinate.content[0] && word.Length == 1)
            {
                return (true, coordinate);
            }
            (int maxX, int maxY) = (matrix.Max(x => x.x), matrix.Max(x => x.y));

            if (coordinate.x < maxX && coordinate.y < maxY)
            {
                if (word[0] == coordinate.content[0])
                {
                    MoveDiagonalRight(matrix.First(x => x.x == coordinate.x + 1 && x.y == coordinate.y + 1), matrix, word[1..]);
                }

            }

            return (false, coordinate);
        }

        static (bool, Grid) MoveDiagonalLeft(Grid coordinate, List<Grid> matrix, string word)
        {
            if (word[^1] == coordinate.content[0] && word.Length == 1)
            {
                return (true, coordinate);
            }
            (int maxX, int maxY) = (matrix.Max(x => x.x), matrix.Max(x => x.y));

            if (coordinate.x > 1 && coordinate.y < maxY)
            {
                if (word[0] == coordinate.content[0])
                {
                    MoveDiagonalLeft(matrix.First(x => x.x == coordinate.x - 1 && x.y == coordinate.y + 1), matrix, word[1..]);
                }

            }

            return (false, coordinate);
        }

        static (bool, Grid) MoveVertical(Grid coordinate, List<Grid> matrix, string word)
        {
            if (word[^1] == coordinate.content[0] && word.Length == 1)
            {
                return (true, coordinate);
            }
            (int maxX, int maxY) = (matrix.Max(x => x.x), matrix.Max(x => x.y));
            if (coordinate.y < maxY)
            {
                if (word[0] == coordinate.content[0])
                {
                    return MoveVertical(matrix.First(x => x.x == coordinate.x && x.y == coordinate.y + 1), matrix, word[1..]);
                }
            }

            return (false, coordinate);
        }



        static (bool,Grid) MoveHorizontal(Grid coordinate, List<Grid> matrix, string word)
        {
            if (word[^1] == coordinate.content[0] && word.Length == 1)
            {
                return (true, coordinate);
            }
            (int maxX, int maxY) = (matrix.Max(x => x.x), matrix.Max(x => x.y));
            if (coordinate.x < maxX)
            {
                if (word[0] == coordinate.content[0])
                {
                    return MoveHorizontal(matrix.First(x => x.x == coordinate.x + 1 && x.y == coordinate.y ), matrix, word[1..]);
                }
            }

            return (false, coordinate);
        }
        public static List<int> FindWords(List<Grid> grid, string word, bool reversed = false)
        {
             List<int>? ids = null;
          
            var starts = grid.Where(x => x.content[0] == word[0]);
            foreach (var coordinate in starts)
            {
                var horizontal = MoveHorizontal(coordinate, grid, word);
                var vertical = MoveVertical(coordinate, grid, word);
                var diaLeft = MoveDiagonalLeft(coordinate, grid, word);
                var diaRight = MoveDiagonalRight(coordinate, grid, word);
                if (horizontal.Item1 )
                    return GetIds(coordinate, horizontal.Item2, word, 1, 0, grid);
                
                if (vertical.Item1)
                    return GetIds(coordinate, vertical.Item2, word, 0, 1, grid);
                if(diaRight.Item1)
                    return GetIds(coordinate, diaRight.Item2, word, 1, 1, grid);

                if(diaLeft.Item1)
                    return GetIds(coordinate, diaLeft.Item2, word, -1, 1, grid);
                
                if (!reversed && coordinate.id == starts.Last().id)
                    return FindWords(grid, Reverse(word), true);
            }


            return ids;
        }

        static List<int> GetIds(Grid start, Grid end, string word, int cx, int cy , List<Grid> matrix)
        {
            Console.Write(word);
            var ids = new List<int>();
            ids.Add(start.id);
            for (int i = 1; i < word.Length; i++)
            {
                ids.Add(matrix.First(x => x.x == start.x + (i*cx) && x.y == start.y + (i*cy)).id);
            }

            Console.WriteLine();
            return ids;
        }

        static string Reverse(string s)
        {
            string reverseString = string.Empty;
            foreach (char c in s)
            {
                reverseString = c + reverseString;
            }

            return reverseString;
        }
        public static string Decrypt(String cipherText, int shiftKey)
        {
            string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
            cipherText = cipherText.ToLower();
            String message = "";
            for (int ii = 0; ii < cipherText.Length; ii++)
            {
                int charPosition = ALPHABET.IndexOf(cipherText[ii]);
                int keyVal = (charPosition - shiftKey) % 26;
                if (keyVal < 0)
                {
                    keyVal = ALPHABET.Length + keyVal;
                }
                char replaceVal = ALPHABET[keyVal];
                message += replaceVal;
            }
            return message;
        }

    }
}