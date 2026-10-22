using Microsoft.JSInterop;

namespace CalcTools.Web.Services;

public interface IClipboardService
{
    public Task<string> GetAsync();
    public Task SetAsync(string text);
}

public class ClipboardService(IJSRuntime jsRuntime) : IClipboardService
{
    public async Task<string> GetAsync()
        => await jsRuntime.InvokeAsync<string>("navigator.clipboard.readText");

    public async Task SetAsync(string text) =>
        await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
}