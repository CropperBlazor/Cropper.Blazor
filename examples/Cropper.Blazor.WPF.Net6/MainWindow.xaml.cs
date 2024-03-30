using System.Windows;
using Cropper.Blazor.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace Cropper.Blazor.WPF.Net6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // https://www.infoq.com/news/2021/04/dotnet-6-webview-winforms-wpf/
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();
            serviceCollection.AddCropper();
            serviceCollection.AddMudServices();

#if DEBUG
            serviceCollection.AddBlazorWebViewDeveloperTools();
#endif
            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
    }
}