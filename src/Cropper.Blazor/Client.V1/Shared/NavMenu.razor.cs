using Cropper.Blazor.Client.Extensions;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Cropper.Blazor.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] private IMenuService MenuService { get; set; } = null!;

        [Inject] private NavigationManager NavMan { get; set; } = null!;

        private string? Section;

        protected override void OnInitialized()
        {
            Refresh();
            base.OnInitialized();
        }

        public void Refresh()
        {
            Section = NavMan.GetSection();
            StateHasChanged();
        }
    }
}
