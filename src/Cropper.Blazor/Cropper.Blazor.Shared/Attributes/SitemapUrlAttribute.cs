using Cropper.Blazor.Shared.Models;

namespace Cropper.Blazor.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SitemapUrlAttribute : Attribute
    {
        public ChangeFreq ChangeFreq { get; }
        public double Priority { get; }
        public string[] Urls { get; }

        public SitemapUrlAttribute(
            string urls = null,
            ChangeFreq changeFreq = ChangeFreq.Daily,
            double priority = 0.5)
        {
            Urls = urls.Split(",");
            ChangeFreq = changeFreq;
            Priority = priority;
        }
    }
}
