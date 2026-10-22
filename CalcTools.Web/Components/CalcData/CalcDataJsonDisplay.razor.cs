using CalcTools.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CalcTools.Web.Components.CalcData;

public partial class CalcDataJsonDisplay : ComponentBase
{
    [Parameter] public string? Json { get; set; }
    [Parameter] public EventCallback<string> JsonChanged { get; set; }
    [Parameter] public string? LogFileName { get; set; }
    [Parameter] public EventCallback<string?> LogFileNameChanged { get; set; }

    [Inject] private IFileDownloadService FileDownloadService { get; set; } = null!;
    [Inject] private IClipboardService ClipboardService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    private async Task DownloadAsync()
    {
        if (string.IsNullOrWhiteSpace(Json))
            return;

        var fileName = !string.IsNullOrWhiteSpace(LogFileName)
            ? $"{Path.GetFileNameWithoutExtension(LogFileName)}.calc.json"
            : "calc.json";

        await FileDownloadService.DownloadFile(fileName, Json);
    }

    private async Task CopyAsync()
    {
        if (string.IsNullOrWhiteSpace(Json))
            return;

        await ClipboardService.SetAsync(Json);
        Snackbar.Add("Calc Data Copied", Severity.Success, cfg => { cfg.Icon = Icons.Material.Outlined.ContentCopy; });
    }
}