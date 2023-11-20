namespace Cropper.Blazor.Sitemap.Generator
{
    public class Paths
    {
        private const string SitemapDirectory = "wwwroot";

        public static string SrcDirPath
        {
            get
            {
                var workingPath = Path.GetFullPath(".");
                do
                {
                    workingPath = Path.GetDirectoryName(workingPath);
                }
                while (Path.GetFileName(workingPath) != "Cropper.Blazor" && !string.IsNullOrWhiteSpace(workingPath));

                return workingPath;
            }
        }

        public static string SitemapDirPath
        {
            get
            {
                return Directory.EnumerateDirectories(SrcDirPath, Path.Combine("Client", SitemapDirectory)).FirstOrDefault();
            }
        }
    }
}
