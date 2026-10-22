using Microsoft.JSInterop;

namespace CalcTools.Web.Services;

public interface ILocalStorageService
{
    public Task<List<string>> GetKeysAsync();
    public Task<string> GetAsync(string key);
    public Task SetAsync(string key, string value);
    public Task RemoveAsync(string key);
    public Task ClearAsync();
}

public class LocalStorageService(IJSRuntime js) : ILocalStorageService
{
    public async Task<List<string>> GetKeysAsync() =>
        await js.InvokeAsync<List<string>>("getLocalStorageKeys");

    public async Task<string> GetAsync(string key) =>
        await js.InvokeAsync<string>("getLocalStorageItem", key);

    public async Task SetAsync(string key, string value) =>
        await js.InvokeVoidAsync("setLocalStorageItem", key, value);

    public async Task RemoveAsync(string key) =>
        await js.InvokeVoidAsync("removeLocalStorageItem", key);

    public async Task ClearAsync() =>
        await js.InvokeVoidAsync("clearLocalStorage");
}