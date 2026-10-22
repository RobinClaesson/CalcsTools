using MudBlazor;

namespace CalcTools.Web.Layout;

public partial class MainLayout
{
    private static readonly MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["pokemon-emerald"],
                FontSize = "1.25rem"
            },
            H1 = new H1Typography { FontSize = "2.75rem" },
            H2 = new H2Typography { FontSize = "2rem" },
            H3 = new H3Typography { FontSize = "1.5rem" },
            Body1 = new Body1Typography { FontSize = "1.25rem" },
            Body2 = new Body2Typography { FontSize = "1rem" },
            Button = new ButtonTypography { FontSize = "1.25rem" }
        }
    };
};