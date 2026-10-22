using System.Text;
using Microsoft.JSInterop;

namespace CalcTools.Web.Services;

public interface IFileDownloadService
{
    Task DownloadFile(string filename, string fileContent);
}

public class FileDownloadService(IJSRuntime js) : IFileDownloadService
{
    public async Task DownloadFile(string filename, string fileContent)
    {
        var bytes = Encoding.UTF8.GetBytes(fileContent);
        var fileStream = new MemoryStream(bytes);
        using var streamRef = new DotNetStreamReference(stream: fileStream);
        
        await js.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
    }
}