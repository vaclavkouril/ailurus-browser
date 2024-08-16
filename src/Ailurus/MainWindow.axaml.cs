using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;


namespace Ailurus
{
    public partial class MainWindow : Window
    {
        private readonly TabController _tabController;

        public MainWindow()
        {
            InitializeComponent();

            var tabControl = this.FindControl<TabControl>("tabControl");
            _tabController = new TabController(tabControl);
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
            _tabController.AddTab("https://www.example.com"); // Example URL or start page
        }
    }
}
