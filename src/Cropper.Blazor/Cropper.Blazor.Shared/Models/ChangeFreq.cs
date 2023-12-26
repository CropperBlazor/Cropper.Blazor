using System.ComponentModel;

namespace Cropper.Blazor.Shared.Models
{
    public enum ChangeFreq
    {
        [Description("always")]
        Always,

        [Description("hourly")]
        Hourly,

        [Description("daily")]
        Daily,

        [Description("weekly")]
        Weekly,

        [Description("monthly")]
        Monthly,

        [Description("yearly")]
        Yearly,

        [Description("never")]
        Never
    }
}
