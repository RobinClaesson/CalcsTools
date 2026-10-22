using CalcTools.Core.NPoint;

namespace CalcTools.Web.Services;

public class LocalStorageDocumentCache(ILocalStorageService localStorage) : IDocumentCache
{
    public async Task<bool> ContainsAsync(string documentId)
    {
        var keys = await localStorage.GetKeysAsync();
        return keys.Contains(documentId);
    }

    public Task<string> GetAsync(string documentId) => localStorage.GetAsync(documentId);
    
    public Task SetAsync(string documentId, string document) => localStorage.SetAsync(documentId, document);
    
    public Task ClearAsync() => localStorage.ClearAsync();
}