using System.Diagnostics;
using System.Xml;
using Cropper.Blazor.Shared.Models;
using Cropper.Blazor.Sitemap.Generator;
using Cropper.Blazor.Sitemap.Generator.Extensions;
using Cropper.Blazor.Sitemap.Generator.Services;

public class Program
{
    public static int Main()
    {
        Stopwatch stopWatch = Stopwatch.StartNew();

        List<SitemapEntry> entries = new SitemapGenerator()
            .GenerateSitemapEntries();

        SaveSitemapEntries(entries);

        Console.WriteLine($"Cropper.Blazor.Sitemap.Generator completed in {stopWatch.ElapsedMilliseconds} msecs");

        return 1;
    }

    static void SaveSitemapEntries(List<SitemapEntry> entries)
    {
        // Create an XML document
        XmlDocument xmlDoc = new XmlDocument();

        // Create the root element
        XmlElement root = xmlDoc.CreateElement("urlset");
        root.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
        xmlDoc.AppendChild(root);

        // Add each URL element
        foreach (var entry in entries)
        {
            AddUrlElement(
                xmlDoc,
                root,
                entry.Url,
                entry.LastModified.ToString(),
                entry.ChangeFrequency.GetDescription(),
                entry.Priority.ToString());
        }

        // Save the XML document to a file in the /wwwroot folder
        xmlDoc.Save(Path.Combine(Paths.SitemapDirPath, "sitemap.xml"));

        Console.WriteLine($"XML file saved to: {Paths.SitemapDirPath}");
    }

    static void AddUrlElement(
        XmlDocument xmlDoc,
        XmlElement root,
        string loc,
        string lastMod,
        string changeFreq,
        string priority)
    {
        // Create the URL element
        XmlElement urlElement = xmlDoc.CreateElement("url");

        // Create and append child elements
        XmlElement locElement = xmlDoc.CreateElement("loc");
        locElement.InnerText = loc;
        urlElement.AppendChild(locElement);

        XmlElement lastmodElement = xmlDoc.CreateElement("lastmod");
        lastmodElement.InnerText = lastMod;
        urlElement.AppendChild(lastmodElement);

        XmlElement changefreqElement = xmlDoc.CreateElement("changefreq");
        changefreqElement.InnerText = changeFreq;
        urlElement.AppendChild(changefreqElement);

        XmlElement priorityElement = xmlDoc.CreateElement("priority");
        priorityElement.InnerText = priority;
        urlElement.AppendChild(priorityElement);

        // Append the URL element to the root
        root.AppendChild(urlElement);
    }
}
