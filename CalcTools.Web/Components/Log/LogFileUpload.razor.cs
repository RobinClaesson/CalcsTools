using CalcTools.Core.UprFvx;
using CalcTools.Web.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CalcTools.Web.Components.Log;

public partial class LogFileUpload : LogComponentBase
{
    private const long TwoMegaByte = 2048000L;

    private IBrowserFile? _randomizerLogFile;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    
    private async Task ReadRandomizerLog()
    {
        if (_randomizerLogFile is null)
            return;

        try
        {
            await using var logStream = _randomizerLogFile.OpenReadStream(maxAllowedSize: TwoMegaByte);
        
            Log = await UprLogParser.ParseLogAsync(logStream);
            await LogChanged.InvokeAsync(Log);

            FileName = _randomizerLogFile.Name;
            await FileNameChanged.InvokeAsync(FileName);
        }
        catch (Exception e)
        {
            Snackbar.Error("Could not read randomizer log file.", e);
        }
    }

    private static string GetContextFileName(IBrowserFile? file) => file?.Name ?? "No file selected";
}