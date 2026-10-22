using CalcTools.Core.NPoint;
using CalcTools.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

namespace CalcTools.Web;

public static class Configuration
{
    public static void RegisterServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMudServices(cfg =>
        {
            cfg.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
            cfg.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            cfg.SnackbarConfiguration.PreventDuplicates = true;
        });
        
        builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
        builder.Services.AddScoped<IDocumentCache, LocalStorageDocumentCache>();
        builder.Services.AddScoped<INPointIoClient, NPointIoClient>();
        builder.Services.AddScoped<IFileDownloadService, FileDownloadService>();
        builder.Services.AddScoped<IClipboardService, ClipboardService>();
    }
}