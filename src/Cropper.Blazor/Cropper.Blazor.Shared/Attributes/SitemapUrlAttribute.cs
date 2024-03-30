using Cropper.Blazor.Shared.Models;

namespace Cropper.Blazor.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SitemapUrlAttribute : Attribute
    {
        public ChangeFreq ChangeFreq { get; }
        public double Priority { get; }
        public string? Url { get; }

        public SitemapUrlAttribute(
            string? url = null,
            ChangeFreq changeFreq = ChangeFreq.Daily,
            double priority = 0.5)
        {
            Url = url;
            ChangeFreq = changeFreq;
            Priority = priority;
        }
    }
}
