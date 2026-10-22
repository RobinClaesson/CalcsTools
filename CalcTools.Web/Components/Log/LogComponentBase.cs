using CalcTools.Core.UprFvx;
using Microsoft.AspNetCore.Components;

namespace CalcTools.Web.Components.Log;

public abstract class LogComponentBase : ComponentBase
{
    [Parameter] public RandomizerLog? Log { get; set; }
    [Parameter] public EventCallback<RandomizerLog?> LogChanged { get; set; }
    [Parameter] public string? FileName { get; set; }
    [Parameter] public EventCallback<string?> FileNameChanged { get; set; }
}