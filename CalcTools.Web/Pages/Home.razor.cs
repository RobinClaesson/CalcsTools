using System.Text.Json;
using CalcTools.Core;
using CalcTools.Core.HzlaCalc;
using CalcTools.Core.NPoint;
using CalcTools.Core.UprFvx;
using CalcTools.Web.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace CalcTools.Web.Pages;

public partial class Home : ComponentBase
{
    [Inject] private INPointIoClient NPointIoClient { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private RandomizerLog? _log;
    private string? _logFileName;
    private CalcData? _calcData;
    private string? _calcDataJson;

    private async Task OnCreateCalcDataClick(MouseEventArgs args)
    {
        if (_log is null)
            return;

        try
        {
            //TODO: Infer base data from log
            _calcData = await NPointIoClient.GetAsync<CalcData>(CalcDataDocuments.Emerald);

            if (_calcData is null)
            {
                Snackbar.Error("Could not read base calc data");
                return;
            }
            
            CalcDataModifier.ApplyRandomizerChanges(_calcData, _log);
            CalcDataModifier.FixTrainerNames(_calcData);

            _calcDataJson = JsonSerializer.Serialize(_calcData);
        }
        catch (Exception e)
        {
            Snackbar.Error("Could not create calc data", e);
        }
    }
}