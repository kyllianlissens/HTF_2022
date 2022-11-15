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
            var testData = await clientInstance.client.GetFromJsonAsync<Caesar>(testUrl);
            Console.WriteLine($"Test endpoint data: {string.Join("; ", testData)}");
            var testSolution = "";
            var decryptedWords = testData.cipheredWords.Select(x => Decrypt(x, 10));
            var ids = new List<int>();
            foreach (var decryptedWord in decryptedWords)
            {
                Console.WriteLine(decryptedWord);
                foreach (var id in FindWords(testData.grid, decryptedWord))
                {
                   ids.Add(id); 
                }

                Console.WriteLine();
            }

     

            //Console.WriteLine(testSolution);
            //var testPostResponse = await clientInstance.client.PostAsJsonAsync<string>(testUrl, testSolution);
            //var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
            //Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
        }

        internal static void ProductionExecution()
        {
            Console.WriteLine("-Production Execution: \n");
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

            if (coordinate.x < 0 && coordinate.y < maxY)
            {
                if (word[0] == coordinate.content[0])
                {
                    MoveDiagonalRight(matrix.First(x => x.x == coordinate.x - 1 && x.y == coordinate.y + 1), matrix, word[1..]);
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
            var ids = new List<int>();
            if (word == "class")
            {
                Console.WriteLine();
            }
            var starts = grid.Where(x => x.content[0] == word[0]);
            foreach (var coordinate in starts)
            {
                if (coordinate.y == 15 && word == "class")
                {
                    Console.WriteLine();
                }
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
                    return GetIds(coordinate, diaLeft.Item2, word, 1, 1, grid);
                
                if (!reversed)
                    return FindWords(grid, Reverse(word), true);
            }


            return ids;
        }

        static List<int> GetIds(Grid start, Grid end, string word, int cx, int cy , List<Grid> matrix)
        {
            Console.Write(start.content);
            var ids = new List<int>();
            ids.Add(start.id);
            for (int i = 1; i < word.Length; i++)
            {
                Console.Write(matrix.First(x => x.x == start.x + (i * cx) && x.y == start.y + (i * cy)).content);
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