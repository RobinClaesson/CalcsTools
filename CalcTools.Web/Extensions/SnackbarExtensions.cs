using MudBlazor;

namespace CalcTools.Web.Extensions;

public static class SnackbarExtensions
{
    extension(ISnackbar snackbar)
    {
        public Snackbar? Error(string message, Exception? exception = null)
        {
            if (exception is null)
                return snackbar.Add(message, Severity.Error);

            Console.WriteLine($"{exception.GetType()}: {exception.Message}");
            return snackbar.Add($"{message.Trim('.', ' ')}. See console for details", Severity.Error);
        }
    }
}