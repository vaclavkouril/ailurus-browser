using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.ReactiveUI;
using Ailurus.ViewModels;

namespace Ailurus
{
    public partial class MainWindow : Window
    {
        
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // Open a default tab on startup
            viewModel.AddNewTabCommand.Execute().Subscribe();
        }
    }
}