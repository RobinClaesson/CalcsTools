namespace CalcTools.Core.NPoint;

public interface IDocumentCache
{
    Task<bool> ContainsAsync(string documentId);
    Task<string> GetAsync(string documentId);
    Task SetAsync(string documentId, string document);
    Task ClearAsync();
}