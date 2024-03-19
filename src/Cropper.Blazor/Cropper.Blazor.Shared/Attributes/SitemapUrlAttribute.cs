using Cropper.Blazor.Shared.Models;

namespace Cropper.Blazor.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SitemapUrlAttribute(
        string? url = null,
        ChangeFreq changeFreq = ChangeFreq.Daily,
        double priority = 0.5) : Attribute
    {
        public ChangeFreq ChangeFreq { get; } = changeFreq;
        public double Priority { get; } = priority;
        public string? Url { get; } = url;
    }
}
