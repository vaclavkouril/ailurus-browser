using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Ailurus.ViewModels;


namespace Ailurus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Assuming DataContext is set in XAML
            var viewModel = DataContext as MainWindowViewModel;
            if (viewModel == null)
            {
                throw new InvalidOperationException("MainWindowViewModel not found in DataContext.");
            }

            // Open a default tab on startup
            viewModel.AddNewTabCommand.Execute().Subscribe();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var urlBox = this.FindControl<TextBox>("urlBox");
            if (urlBox != null && Uri.TryCreate(urlBox.Text, UriKind.Absolute, out var uri))
            {
                var viewModel = DataContext as MainWindowViewModel;
                if (viewModel != null && viewModel.SelectedTab != null)
                {
                    viewModel.SelectedTab.Content = uri.ToString(); // Assuming Content is of type string (URI)
                }
            }
        }

        private void NewTabButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel?.AddNewTabCommand.Execute().Subscribe();
        }
    }
}