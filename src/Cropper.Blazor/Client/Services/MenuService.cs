using Cropper.Blazor.Client.Models;

namespace Cropper.Blazor.Client.Services
{
    public interface IMenuService
    {
        //Menu sections
        IEnumerable<DocsLink> Examples { get; }
    }

    /// <summary>
    /// The aim of this class is to add new items to NavMenu
    /// </summary>
    public class MenuService : IMenuService
    {
        private IEnumerable<DocsLink> _examples;
        /// <summary>
        /// Examples menu links
        /// </summary>
        public IEnumerable<DocsLink> Examples => _examples ??= new List<DocsLink>
            {
                new DocsLink {Title = "One Cropper", Href = "examples/cropperusage"}
            }.OrderBy(x => x.Title);
    }
}
