using System.Text.RegularExpressions;

namespace Library.Api.ErrorHandling;

public static class SlugHelper
{
    public static string Generate(string input)
    {
        input = input.ToLower().Trim();
        input = Regex.Replace(input, @"[^a-z0-9\s-]", "");
        input = Regex.Replace(input, @"\s+", "-");
        return input;
    }
}
