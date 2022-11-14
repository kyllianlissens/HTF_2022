using System;
using System.Collections.Generic;

namespace HTF2022
{
    internal static class Utilities
    {
        internal static List<int> randomIntegerList(int amount, int min, int max)
        {
            var numbers = new List<int>();
            var rnd = new Random();
            for (var i = 0; i <= amount; i++) numbers.Add(rnd.Next(min, max));
            return numbers;
        }
    }
}