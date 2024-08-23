using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel representing a single browser tab in the application.
    /// </summary>
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header = "New Tab";
        private string _url = string.Empty;
        private bool _isLoading;
        private CefGlueBrowserControl _content;

        /// <summary>
        /// Gets or sets the title of the browser tab.
        /// </summary>
        public string Header
        {
            get => _header;
            set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        /// <summary>
        /// Gets or sets the URL currently being displayed in the browser tab.
        /// </summary>
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the browser tab is currently loading content.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => this.RaiseAndSetIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Gets or sets the content of the browser tab, which is an instance of <see cref="CefGlueBrowserControl"/>.
        /// </summary>
        public CefGlueBrowserControl Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        /// <summary>
        /// Command that closes the browser tab.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserTabViewModel"/> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The MainWindowViewModel that manages this tab.</param>
        public BrowserTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _content = new CefGlueBrowserControl();
            // Initialize the content with the current tab context
            _content.Initialize(this);
            CloseTabCommand = ReactiveCommand.Create(() => mainWindowViewModel.CloseTab(this));
        }

        /// <summary>
        /// Navigates the browser in this tab to the specified URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public void Navigate(string url)
        {
            _content.Navigate(url);
        }
    }


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

        private string _url = string.Empty;
        /// <summary>
        /// Gets or sets the URL currently displayed in the address bar.
        /// </summary>
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
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
            var newTab = new BrowserTabViewModel(this) { Header = "New Tab" };
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        /// <summary>
        /// Navigates the currently selected tab to the URL entered in the address bar.
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
