using System.Globalization;

namespace CalcTools.Core.Extensions;

public static class StringExtensions
{
    public static char CalcApostrophe { get; set; } = '\'';
    extension(string s)
    {
        public string NormalizePokemonName() => s
            .Replace("♀", "-F")
            .Replace("♂", "-M")
            .Replace('’', CalcApostrophe);
        
        public bool HasUnicodeError() => s.Contains('\ufffd');

        public string FixUnicodeCharachters() => s.Replace("\ufffd", "é");

        public string ToTitleCase() => CultureInfo.CurrentCulture.TextInfo
            .ToTitleCase(s.ToLower(CultureInfo.CurrentCulture));
    }
}