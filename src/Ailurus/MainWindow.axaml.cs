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
            
            InitializeBrowser();
        } 
        
        private void InitializeBrowser()
        {
            _browser = new AvaloniaCefBrowser
            {
                
            };
            _browser.ShowDeveloperTools();

            var panel = this.FindControl<Grid>("MainPanel"); // Use Grid instead of Panel
            if (panel != null)
            {
                panel.Children.Add(_browser);
                panel.InvalidateMeasure();
                panel.InvalidateArrange();

                _browser.Address = "https://www.google.com";
            }

            // Subscribe to the LayoutUpdated event to adjust browser size
            this.LayoutUpdated += OnLayoutUpdated;
        }

        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            if (_browser != null)
            {
                var panel = this.FindControl<Grid>("MainPanel");
                if (panel != null)
                {
                    // Adjust the browser size to match the panel's size
                    _browser.Width = panel.Bounds.Width;
                    _browser.Height = panel.Bounds.Height;
                }
            }
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