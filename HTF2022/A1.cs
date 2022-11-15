using System.Net.Http.Json;
using System.Text;

namespace HTF2022;

public static class Roman
{
    public static readonly Dictionary<char, int> RomanNumberDictionary;
    public static readonly Dictionary<int, string> NumberRomanDictionary;

    static Roman()
    {
        RomanNumberDictionary = new Dictionary<char, int>
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        NumberRomanDictionary = new Dictionary<int, string>
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
    }

    public static string To(int number)
    {
        var roman = new StringBuilder();

        foreach (var item in NumberRomanDictionary)
            while (number >= item.Key)
            {
                roman.Append(item.Value);
                number -= item.Key;
            }

        return roman.ToString();
    }

    public static int From(string roman)
    {
        var total = 0;

        var previousRoman = '\0';

        foreach (var t in roman)
        {
            var previous = previousRoman != '\0' ? RomanNumberDictionary[previousRoman] : '\0';
            var current = RomanNumberDictionary[t];

            if (previous != 0 && current > previous)
                total = total - 2 * previous + current;
            else
                total += current;

            previousRoman = t;
        }

        return total;
    }
}

internal static class A1
{
    private const string TestUrl = "/api/path/1/easy/Sample";
    private const string ProductionUrl = "/api/path/1/easy/Puzzle";

    private static readonly HTTPInstance ClientInstance = new();

    internal static void LocalExecution()
    {
        Console.WriteLine("-Local Execution: \n");
        //test RomanToNumber
        Console.WriteLine("RomanFrom(\"IL\") = " + Roman.From("IL"));
        Console.WriteLine("RomanFrom(\"XLVIII\") = " + Roman.From("XLVIII"));
    }

    internal static async Task TestExecution()
    {
        Console.WriteLine("-Test Execution: \n");
        var response = await ClientInstance.Client.GetFromJsonAsync<List<string>>(TestUrl);

        foreach (var item in response) Console.WriteLine("RomanFrom(\"" + item + "\") = " + Roman.From(item));
        var numbers = response.Select(Roman.From).ToList();
        var sum = numbers.Sum();
        Console.WriteLine("Sum of all numbers = " + sum);
        Console.WriteLine("Sum in Roman = " + Roman.To(sum));

        var testPostResponse = await ClientInstance.Client.PostAsJsonAsync(TestUrl, Roman.To(sum));
        var testPostResponseValue = await testPostResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Test endpoint response: {testPostResponseValue}");
    }

    internal static async Task ProductionExecution()
    {
        Console.WriteLine("-Production Execution: \n");
        var response = await ClientInstance.Client.GetFromJsonAsync<List<string>>(ProductionUrl);
        Console.WriteLine("Response: ");
        foreach (var item in response) Console.WriteLine("RomanFrom(\"" + item + "\") = " + Roman.From(item));
        var numbers = response.Select(Roman.From).ToList();
        var sum = numbers.Sum();
        Console.WriteLine("Sum of all numbers = " + sum);
        Console.WriteLine("Sum in Roman = " + Roman.To(sum));

        var productionPostResponse = await ClientInstance.Client.PostAsJsonAsync(ProductionUrl, Roman.To(sum));
        var productionPostResponseValue = await productionPostResponse.Content.ReadAsStringAsync();
        Console.WriteLine($"Production endpoint response: {productionPostResponseValue}");
    }
}