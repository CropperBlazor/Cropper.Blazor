using System.Diagnostics;
using Cropper.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Interfaces;

namespace Cropper.Blazor.Client.Components.Docs
{
    public partial class DocsPage : ComponentBase
    {
        private const char NumberSignSymbol = '#';
        private readonly Queue<DocsSectionLink> _bufferedSections = new();
        private MudPageContentNavigation ContentNavigation;
        private Stopwatch Stopwatch = Stopwatch.StartNew();
        private string? Anchor = null;
        private bool IsDisplayView;
        private string? ComponentName;
        private bool IsRenderAds;
        [Inject] NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRenderQueueService RenderQueue { get; set; } = null!;
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool DisplayFooter { get; set; }

        private bool _contentDrawerOpen = true;
        public event Action<Stopwatch> Rendered;
        private readonly Dictionary<DocsPageSection, MudPageContentSection> _sectionMapper = [];

        private int _sectionCount;

        public int SectionCount
        {
            get
            {
                lock (this)
                {
                    return _sectionCount;
                }
            }
        }

        public int IncrementSectionCount()
        {
            lock (this)
            {
                return _sectionCount++;
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RenderQueue.Clear();
            string relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

            if (relativePath.Contains(NumberSignSymbol) == true)
            {
                Anchor = relativePath.Split(NumberSignSymbol, StringSplitOptions.RemoveEmptyEntries)[1];
            }
        }

        protected override void OnParametersSet()
        {
            Stopwatch = Stopwatch.StartNew();
            _sectionCount = 0;

            /*for after this release is done*/
            IsDisplayView = false;
            ComponentName = "temp";
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (Stopwatch.IsRunning)
            {
                Stopwatch.Stop();
                Rendered?.Invoke(Stopwatch);
            }

            if (firstRender)
            {
                IsRenderAds = true;
                StateHasChanged();
            }
        }

        public string GetParentTitle(DocsPageSection section)
        {
            if (section == null)
            {
                return string.Empty;
            }

            if (section == null || section.ParentSection == null ||
                _sectionMapper.ContainsKey(section.ParentSection) == false)
            {
                return string.Empty;
            }

            var item = _sectionMapper[section.ParentSection];

            return item.Title;
        }

        internal async void AddSection(DocsSectionLink sectionLinkInfo, DocsPageSection section)
        {
            _bufferedSections.Enqueue(sectionLinkInfo);

            if (ContentNavigation != null)
            {
                while (_bufferedSections.Count > 0)
                {
                    DocsSectionLink item = _bufferedSections.Dequeue();

                    if (ContentNavigation.Sections.FirstOrDefault(x => x.Id == sectionLinkInfo.Id) == default)
                    {
                        MudPageContentSection? parentInfo = null;

                        if (section.ParentSection != null && _sectionMapper.ContainsKey(section.ParentSection) == true)
                        {
                            parentInfo = _sectionMapper[section.ParentSection];
                        }

                        MudPageContentSection info =
                            new(sectionLinkInfo.Title, sectionLinkInfo.Id, section.Level,
                                parentInfo);

                        _sectionMapper.Add(section, info);
                        ContentNavigation.AddSection(info, false);
                    }
                }

                ((IMudStateHasChanged)ContentNavigation).StateHasChanged();

                if (Anchor != null)
                {
                    if (sectionLinkInfo.Id == Anchor)
                    {
                        await ContentNavigation.ScrollToSection(new Uri(NavigationManager.Uri));
                        Anchor = null;
                    }
                }
            }
        }
    }
}
