namespace Cropper.Blazor.Client.Pages
{
    public partial class Index
    {
        public Dictionary<string, object> LogoInputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "alt", "Cropper.Blazor logo" }
            };
    }
}
