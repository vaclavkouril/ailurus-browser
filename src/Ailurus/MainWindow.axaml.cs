using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ailurus
{
    public partial class MainWindow : Window
    {
        private CefGlueBrowserControl? _webView;

        
        public MainWindow()
        {
            try
            {
                Console.WriteLine("MainWindow constructor started.");
                InitializeComponent();
                Console.WriteLine("MainWindow constructor completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in MainWindow constructor: " + ex);
                throw;
            }
        }

        private void InitializeComponent()
        {
            try
            {
                Console.WriteLine("Initializing MainWindow components.");
                AvaloniaXamlLoader.Load(this);
                Console.WriteLine("MainWindow components initialized.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in InitializeComponent: " + ex);
                throw;
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try{
                Console.WriteLine("GoButton clicked.");
                var urlBox = this.FindControl<TextBox>("urlBox");
                if (urlBox != null && Uri.TryCreate(urlBox.Text, UriKind.Absolute, out var uri))
                {
                    Console.WriteLine($"Navigating to URL: {uri}");
                    _webView?.Navigate(uri.ToString());
                }
            }
            catch (Exception ex){
                Console.WriteLine("Exception in GoButton_Click: " + ex);
                throw;
            }
        }
    }
}
