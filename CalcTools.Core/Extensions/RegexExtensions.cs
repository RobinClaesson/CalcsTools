using System.Text.RegularExpressions;

namespace CalcTools.Core.Extensions;

public static class RegexExtensions
{
    extension(Capture capture)
    {
        public int IntValue => int.Parse(capture.Value);
    }
}