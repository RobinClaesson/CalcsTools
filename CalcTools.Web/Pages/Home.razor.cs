using System.Text.Json;
using CalcTools.Core;
using CalcTools.Core.HzlaCalc;
using CalcTools.Core.NPoint;
using CalcTools.Core.UprFvx;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CalcTools.Web.Pages;

public partial class Home : ComponentBase
{
    [Inject] private INPointIoClient NPointIoClient { get; set; } = null!;

    private RandomizerLog? _log;
    private string? _logFileName;
    private CalcData? _calcData;
    private string? _calcDataJson;

    private async Task OnCreateCalcDataClick(MouseEventArgs args)
    {
        if (_log is null)
            return;

        //TODO: Infer base data from log
        Console.WriteLine($"{DateTime.Now:hh:mm:ss} Fetching calc data");
        _calcData = await NPointIoClient.GetAsync<CalcData>(CalcDataDocuments.Emerald);

        if (_calcData is null)
            return;
        Console.WriteLine($"{DateTime.Now:hh:mm:ss} Modifying calc data");

        CalcDataModifier.ApplyRandomizerChanges(_calcData, _log);
        CalcDataModifier.FixTrainerNames(_calcData);
        
        Console.WriteLine($"{DateTime.Now:hh:mm:ss} Creating Json");
        _calcDataJson = JsonSerializer.Serialize(_calcData);
        
        Console.WriteLine($"{DateTime.Now:hh:mm:ss} Randomized calc done!");
    }
}