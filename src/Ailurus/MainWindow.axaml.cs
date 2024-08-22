using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using System.Collections.ObjectModel;

namespace Ailurus
{
    public partial class MainWindow : Window
    {
        private readonly TabController _tabController;

        public MainWindow()
        {
            InitializeComponent();
            
            var tabControl = this.FindControl<TabControl>("tabControl");
            if (tabControl == null)
            {
                throw new InvalidOperationException("TabControl not found in the XAML.");
            }

            _tabController = new TabController(tabControl);

            // Open a default tab on startup
            _tabController.AddNewTab("http://example.com");
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var urlBox = this.FindControl<TextBox>("urlBox");
            if (urlBox != null && Uri.TryCreate(urlBox.Text, UriKind.Absolute, out var uri))
            {
                _tabController.NavigateCurrentTab(uri.ToString());
            }
        }

        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            _tabController.AddNewTab("http://example.com"); // Or start with a blank page
        }
    }
}
