using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Pages
{
    public partial class About
    {
        public IEnumerable<TeamMember> TeamMembers => new TeamMember[]
        {
            new TeamMember
            {
                Avatar = "https://avatars.githubusercontent.com/u/50423072?v=4",
                From = "Kyiv, Ukraine",
                Github = "https://github.com/MaxymGorn",
                LinkedIn = "https://www.linkedin.com/in/maxym-gornytskiy/",
                Name = "Maksym Hornytskiy",
                Role = "Creator",
                Languages = "English, Ukrainian"
            },
            new TeamMember
            {
                Avatar = "https://avatars.githubusercontent.com/u/38187349?v=4",
                From = "Ukraine",
                Github = "https://github.com/ColdForeign",
                LinkedIn = string.Empty,
                Name = "George Radchuk",
                Role = "Creator",
                Languages = "English, Ukrainian"
            }
        };

        public Dictionary<string, object> AvatarInputAttributes { get; set; } =
            new Dictionary<string, object>()
            {
                { "loading", "lazy" },
                { "alt", "GitHub avatar image" }
            };
    }
}
