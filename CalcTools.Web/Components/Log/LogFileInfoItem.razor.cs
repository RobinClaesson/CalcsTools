using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CalcTools.Web.Components.Log;

public partial class LogFileInfoItem : ComponentBase
{
    [Parameter] public string Key { get; set; } = string.Empty;
    [Parameter] public object Value { get; set; } = string.Empty;
    [Parameter] public int Xs { get; set; } = 5;
    [Parameter] public Color KeyColor { get; set; } = Color.Info;
    [Parameter] public Color ValueColor { get; set; } = Color.Default;
}