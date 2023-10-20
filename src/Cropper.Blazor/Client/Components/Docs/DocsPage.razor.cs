using System.Diagnostics;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using Cropper.Blazor.Client.Models;
using MudBlazor;
using MudBlazor.Interfaces;

namespace Cropper.Blazor.Client.Components.Docs
{
    public partial class DocsPage : ComponentBase
    {
        [Parameter] public bool DisplayFooter { get; set; }

        private Queue<DocsSectionLink> _bufferedSections = new();
        private MudPageContentNavigation _contentNavigation;
        private Stopwatch _stopwatch = Stopwatch.StartNew();
        private string _anchor = null;
        private bool _displayView;
        private string _componentName;
        private bool _renderAds;
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] private IRenderQueueService RenderQueue { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        private bool _contentDrawerOpen = true;
        public event Action<Stopwatch> Rendered;
        private Dictionary<DocsPageSection, MudPageContentSection> _sectionMapper = new();

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
            RenderQueue.Clear();
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

            /*for after this release is done*/
            _displayView = false;
            _componentName = "temp";
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
                _renderAds = true;
                StateHasChanged();
            }
        }

        public string GetParentTitle(DocsPageSection section)
        {
            if (section == null) { return string.Empty; }

            if (section == null || section.ParentSection == null ||
                _sectionMapper.ContainsKey(section.ParentSection) == false) { return string.Empty; }

            var item = _sectionMapper[section.ParentSection];

            return item.Title;
        }

        internal async void AddSection(DocsSectionLink sectionLinkInfo, DocsPageSection section)
        {
            _bufferedSections.Enqueue(sectionLinkInfo);

            if (_contentNavigation != null)
            {
                while (_bufferedSections.Count > 0)
                {
                    var item = _bufferedSections.Dequeue();

                    if (_contentNavigation.Sections.FirstOrDefault(x => x.Id == sectionLinkInfo.Id) == default)
                    {
                        MudPageContentSection parentInfo = null;
                        if (section.ParentSection != null && _sectionMapper.ContainsKey(section.ParentSection) == true)
                        {
                            parentInfo = _sectionMapper[section.ParentSection];
                        }

                        var info =
                            new MudPageContentSection(sectionLinkInfo.Title, sectionLinkInfo.Id, section.Level,
                                parentInfo);
                        _sectionMapper.Add(section, info);
                        _contentNavigation.AddSection(info, false);
                    }
                }

                ((IMudStateHasChanged)_contentNavigation).StateHasChanged();

                if (_anchor != null)
                {
                    if (sectionLinkInfo.Id == _anchor)
                    {
                        await _contentNavigation.ScrollToSection(new Uri(NavigationManager.Uri));
                        _anchor = null;
                    }
                }
            }
        }
    }
}
