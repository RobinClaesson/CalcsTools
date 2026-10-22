using CalcTools.Core.UprFvx;
using Microsoft.AspNetCore.Components.Forms;

namespace CalcTools.Web.Components.Log;

public partial class LogFileUpload : LogComponentBase
{
    private const long TwoMegaByte = 2048000L;

    private IBrowserFile? _randomizerLogFile;

    private async Task ReadRandomizerLog()
    {
        if (_randomizerLogFile is null)
            return;

        await using var logStream = _randomizerLogFile.OpenReadStream(maxAllowedSize: TwoMegaByte);
        
        Log = await UprLogParser.ParseLogAsync(logStream);
        await LogChanged.InvokeAsync(Log);

        FileName = _randomizerLogFile.Name;
        await FileNameChanged.InvokeAsync(FileName);
    }

    private static string GetContextFileName(IBrowserFile? file) => file?.Name ?? "No file selected";
}