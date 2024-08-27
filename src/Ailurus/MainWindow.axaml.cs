using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Ailurus.ViewModels;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Xilium.CefGlue.Avalonia;

namespace Ailurus
{
    public partial class MainWindow : Window
    {
        private AvaloniaCefBrowser _browser;
        
        public MainWindow()
        {
            InitializeComponent(true);
            
            // Assuming DataContext is set in XAML
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null)
            {
                throw new InvalidOperationException("MainWindowViewModel not found in DataContext.");
            }

            // Open a default tab on startup
            viewModel.AddNewTabCommand.Execute().Subscribe();
            
            _browser = new AvaloniaCefBrowser();
            var panel = this.FindControl<Panel>("MainPanel");
            if (panel != null)
            {
                panel.Children.Add(_browser);
                _browser.Address = "https://www.google.com";
            }
            else
            {
                Console.WriteLine("MainPanel was not found.");
            }
        } 
        private void OnOpenDevTools(object? sender, RoutedEventArgs e)
        {
            _browser.ShowDeveloperTools();
        }
        
        /*
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var urlBox = this.FindControl<TextBox>("urlBox");
            if (urlBox != null && Uri.TryCreate(urlBox.Text, UriKind.Absolute, out var uri))
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null && viewModel.SelectedTab?.Content is CefGlueBrowserControl browserControl)
                {
                    // Use the Navigate method of the CefGlueBrowserControl to navigate to the URL
                    browserControl.Navigate(uri.ToString());
                }
            }
        }

        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel?.AddNewTabCommand.Execute().Subscribe();
        }*/
    }
}