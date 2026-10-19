namespace CalcTools.Core.NPoint;

public interface INPointIoClient
{
    Task<string> GetAsync(string documentId);
    Task<T?> GetAsync<T>(string documentId);
}