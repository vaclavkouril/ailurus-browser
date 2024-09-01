using System;
using Avalonia.Controls;
using Ailurus.ViewModels;

namespace Ailurus;

/// <summary>
/// The main window of the application, responsible for displaying the browser UI.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="viewModel">The view model that will be used as the data context for this window.</param>
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
            
        // open a new tab when the main window is initialized
        viewModel.AddNewTabCommand.Execute().Subscribe();
    }
}