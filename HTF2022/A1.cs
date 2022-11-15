using System.Net.Http.Json;
using System.Text;

namespace HTF2022;
internal static class A1
{
    private const string TestUrl = "/api/path/1/easy/Sample";
    private const string ProductionUrl = "/api/path/1/easy/Puzzle";

    private static readonly HTTPInstance ClientInstance = new();

    internal static void LocalExecution()
    {
        Console.WriteLine("-Local Execution: \n");
        Console.WriteLine("RomanFrom(\"IL\") = " + From("IL"));
        Console.WriteLine("RomanFrom(\"XLVIII\") = " + From("XLVIII"));
    }

    internal static async Task TestExecution()
    {
        Console.WriteLine("-Test Execution: \n");
        var response = await ClientInstance.Client.GetFromJsonAsync<List<string>>(TestUrl);

        foreach (var item in response) Console.WriteLine("RomanFrom(\"" + item + "\") = " + From(item));
        var numbers = response.Select(From).ToList();
        var sum = numbers.Sum();
        Console.WriteLine("Sum of all numbers = " + sum);
        Console.WriteLine("Sum in Roman = " + To(sum));

        var testPostResponse = await ClientInstance.Client.PostAsJsonAsync(TestUrl, To(sum));
        var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
    }

    internal static async Task ProductionExecution()
    {
        Console.WriteLine("-Production Execution: \n");
        var response = await ClientInstance.Client.GetFromJsonAsync<List<string>>(ProductionUrl);
        Console.WriteLine("Response: ");
        foreach (var item in response) Console.WriteLine("RomanFrom(\"" + item + "\") = " + From(item));
        var numbers = response.Select(From).ToList();
        var sum = numbers.Sum();
        Console.WriteLine("Sum of all numbers = " + sum);
        Console.WriteLine("Sum in Roman = " + To(sum));

        var productionPostResponse = await ClientInstance.Client.PostAsJsonAsync(ProductionUrl, To(sum));
        var productionPostResponseValue = await productionPostResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Production endpoint response: {productionPostResponseValue}");
    }
    
    internal static string To(int number)
    {
        var roman = new StringBuilder();

        var numberRomanDictionary = new Dictionary<int, string>
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" }
        };

        foreach (var item in numberRomanDictionary)
            while (number >= item.Key)
            {
                roman.Append(item.Value);
                number -= item.Key;
            }

        return roman.ToString();
    }


    internal static int From(string roman)
    {
        var romanNumberDictionary = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        var total = 0;

        var previousRoman = '\0';

        foreach (var t in roman)
        {
            var previous = previousRoman != '\0' ? romanNumberDictionary[previousRoman] : '\0';
            var current = romanNumberDictionary[t];

            if (previous != 0 && current > previous)
                total = total - 2 * previous + current;
            else
                total += current;

            previousRoman = t;
        }

        return total;
    }
}
