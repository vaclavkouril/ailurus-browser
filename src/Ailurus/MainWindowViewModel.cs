using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel for the main window of the application.
    /// Manages the collection of tabs and handles navigation commands.
    /// </summary>
    public class MainWindowViewModel : ReactiveObject
    {
        /// <summary>
        /// Gets the collection of browser tabs.
        /// </summary>
        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        private BrowserTabViewModel _selectedTab;
        /// <summary>
        /// Gets or sets the currently selected tab.
        /// </summary>
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }

        private string? _url;
        /// <summary>
        /// Gets or sets the URL currently displayed in the address bar.
        /// </summary>
        public string Url
        {
            get => _url;
            set
            {
                Console.WriteLine($"URL changed: Old Value: {_url}, New Value: {value}");
                this.RaiseAndSetIfChanged(ref _url, value);
            }
        }

        /// <summary>
        /// Command to add a new tab.
        /// </summary>
        public ReactiveCommand<Unit, Unit> AddNewTabCommand { get; }

        /// <summary>
        /// Command to navigate the selected tab to the entered URL.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GoCommand { get; }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainWindowViewModel()
        {
            AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
            GoCommand = ReactiveCommand.Create(GoToUrl);
            AddNewTab();  // Adds a default tab on startup
        }

        /// <summary>
        /// Adds a new tab to the browser and sets it as the selected tab.
        /// </summary>
        private void AddNewTab()
        {
            Console.WriteLine("Adding new Tab");
            var newTab = new BrowserTabViewModel(this) { Header = "New Tab" };
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        /// <summary>
        /// Navigates the currently selected tab to the URL filled in the address bar.
        /// </summary>
        private void GoToUrl()
        {
            SelectedTab?.Navigate(Url);
        }

        /// <summary>
        /// Closes the specified tab and switches to another tab if necessary.
        /// </summary>
        /// <param name="tab">The tab to be closed.</param>
        public void CloseTab(BrowserTabViewModel tab)
        {
            Tabs.Remove(tab);
            if (Tabs.Count > 0)
            {
                SelectedTab = Tabs[0];
            }
        }
    }
}
