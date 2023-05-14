using Microsoft.AspNetCore.Components;
using Cropper.Blazor.Client.Services;
using Cropper.Blazor.Client.Extensions;

namespace Cropper.Blazor.Client.Shared
{
    public partial class NavMenu
    {
        [Inject] IMenuService MenuService { get; set; }

        [Inject] NavigationManager NavMan { get; set; }

        string _section;

        protected override void OnInitialized()
        {
            Refresh();
            base.OnInitialized();
        }

        public void Refresh()
        {
            _section = NavMan.GetSection();
            StateHasChanged();
        }
    }
}
