using System.Text.RegularExpressions;

namespace CalcTools.DataBuilder.Js;

public static partial class JsParser
{
    public static string ToJson(string jsObject)
    {
        return JsObjectKeyRegex().Replace(jsObject, "\"${key}\":")
            .Replace('\'', '"');
    }

    public static List<JsVariable> GetJsVariables(string jsScript) =>
        [.. JsVariableRegex().Matches(jsScript).Select(BuildJsVariable)];

    private static JsVariable BuildJsVariable(Match match) =>
        new JsVariable(match.Groups["name"].Value, match.Groups["value"].Value);

    [GeneratedRegex(@"(?<key>\w+):")]
    private static partial Regex JsObjectKeyRegex();

    [GeneratedRegex(
        @"\b(?:var|let|const)\s+(?<name>[A-Za-z_$][\w$]*)\s*=\s*(?<value>.*?);",
        RegexOptions.Singleline)]
    private static partial Regex JsVariableRegex();
}