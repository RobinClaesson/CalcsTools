using Microsoft.AspNetCore.Components;

namespace CalcTools.Web.Components;

public partial class PaperBase : ComponentBase
{
    [Parameter] public string MinWidth { get; set; } = "400px";
    [Parameter] public string MaxWidth { get; set; } = "90%";
    [Parameter] public int Elevation { get; set; } = 5;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Visible { get; set; } = true;
    [Parameter] public int TransitionMs { get; set; } = 1000;

    private string ComponentStyle =>
        $"""
         opacity: {(Visible ? "1" : "0")};
         transition: opacity {TransitionMs}ms ease-in-out;
         """;
}