using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Components.Docs
{
    public partial class DocsPage : ComponentBase
    {
        private Queue<DocsSectionLink> _bufferedSections = new();
        private Stopwatch _stopwatch = Stopwatch.StartNew();
        private string _anchor = null;
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private bool _contentDrawerOpen = true;
        public event Action<Stopwatch> Rendered;

        int _sectionCount;

        public int SectionCount
        {
            get
            {
                lock (this)
                    return _sectionCount;
            }
        }

        public int IncrementSectionCount()
        {
            lock (this)
                return _sectionCount++;
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (relativePath.Contains("#") == true)
            {
                _anchor = relativePath.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
        }

        protected override void OnParametersSet()
        {
            _stopwatch = Stopwatch.StartNew();
            _sectionCount = 0;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                Rendered?.Invoke(_stopwatch);
            }
            if (firstRender)
            {
                StateHasChanged();
            }
        }
    }
}
