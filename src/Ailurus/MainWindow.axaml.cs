using System;
using Avalonia.Controls;

using Ailurus.ViewModels;

namespace Ailurus;

public partial class MainWindow : Window
{
        
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        
            
        viewModel.AddNewTabCommand.Execute().Subscribe();
    }
}