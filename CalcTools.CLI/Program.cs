using System.Text.Json;
using CalcTools.Core;
using CalcTools.Core.HzlaCalc;
using CalcTools.Core.NPoint;
using CalcTools.Core.UprFvx;

var logPath = args[0];

var cache = new FileDocumentCache();
var nPointClient = new NPointIoClient(cache);

var calcData = await nPointClient.GetAsync<CalcData>(CalcDataDocuments.Emerald);
if (calcData is null)
{
    Console.WriteLine("No calc data found");
    return 1;
}

var logContent = File.ReadAllText(logPath);
var log = UprLogParser.ParseLog(logContent);

CalcDataModifier.ApplyRandomizerChanges(calcData, log);
CalcDataModifier.FixTrainerNames(calcData);

await using var outputFile = File.Create("output.json");
await JsonSerializer.SerializeAsync(outputFile, calcData);
return 0;