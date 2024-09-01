using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel for the main window of the Ailurus web browser.
    /// Manages tabs, bookmarks, and key bindings.
    /// </summary>
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ISessionManager _sessionManager;
        private readonly IHistoryManager _historyManager;
        private readonly IBookmarkManager _bookmarkManager;
        private readonly IConfigurationManager _configManager;

        /// <summary>
        /// Gets the collection of browser tabs.
        /// </summary>
        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        /// <summary>
        /// Gets the collection of bookmarks.
        /// </summary>
        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new ObservableCollection<BookmarkItemViewModel>();

        /// <summary>
        /// Gets the collection of dynamically loaded key bindings.
        /// </summary>
        public ObservableCollection<KeyBinding> DynamicKeyBindings { get; } = new ObservableCollection<KeyBinding>();

        private BrowserTabViewModel? _selectedTab;

        /// <summary>
        /// Gets or sets the currently selected tab.
        /// </summary>
        public BrowserTabViewModel? SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab == value) return;

                if (_selectedTab != null)
                {
                    _selectedTab.PropertyChanged -= OnSelectedTabUrlChanged;
                    _selectedTab.PropertyChanged -= OnSelectedTabTitleChanged;
                    _selectedTab.SetIsSelected(false);
                }

                this.RaiseAndSetIfChanged(ref _selectedTab, value);

                if (_selectedTab != null)
                {
                    _selectedTab.SetIsSelected(true);
                    _selectedTab.PropertyChanged += OnSelectedTabUrlChanged;
                    _selectedTab.PropertyChanged += OnSelectedTabTitleChanged;
                }

                UpdateEditableUrl();
                this.RaisePropertyChanged(nameof(BrowserContent));
                UpdateCommands();
            }
        }

        /// <summary>
        /// Gets the browser content of the selected tab.
        /// </summary>
        public AvaloniaCefBrowser? BrowserContent => SelectedTab?.Browser;

        private string _editableUrl = string.Empty;

        /// <summary>
        /// Gets or sets the editable URL in the address bar.
        /// </summary>
        public string EditableUrl
        {
            get => _editableUrl;
            set => this.RaiseAndSetIfChanged(ref _editableUrl, value);
        }

        // Reactive commands for various browser actions
        public ReactiveCommand<Unit, Unit> AddNewTabCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> GoCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ForwardCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> OpenDevToolsCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> OpenHistoryCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> AddBookmarkCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> RefreshSettingsCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateUpCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> NavigateDownCommand { get; private set; }
        
        // Key gestures for various actions, set dynamically
        public KeyGesture BackGesture { get; private set; }
        public KeyGesture ForwardGesture { get; private set; }
        public KeyGesture ReloadGesture { get; private set; }
        public KeyGesture GoGesture { get; private set; }
        public KeyGesture AddBookmarkGesture { get; private set; }
        public KeyGesture OpenHistoryGesture { get; private set; }
        public KeyGesture NavigateUpGesture { get; private set; }
        public KeyGesture NavigateDownGesture { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="sessionManager">The session manager.</param>
        /// <param name="historyManager">The history manager.</param>
        /// <param name="bookmarkManager">The bookmark manager.</param>
        /// <param name="configManager">The configuration manager.</param>
        public MainWindowViewModel(
            ISessionManager sessionManager,
            IHistoryManager historyManager,
            IBookmarkManager bookmarkManager,
            IConfigurationManager configManager)
        {
            _sessionManager = sessionManager;
            _historyManager = historyManager;
            _bookmarkManager = bookmarkManager;
            _configManager = configManager;

            InitializeCommands();
            LoadSessionAsync().ConfigureAwait(false);
            LoadBookmarksAsync().ConfigureAwait(false);
            InitializeKeyBindings().ConfigureAwait(false);
        }

        /// <summary>
        /// Initializes the commands for the main window.
        /// </summary>
        private void InitializeCommands()
        {
            GoCommand = ReactiveCommand.CreateFromTask(GoToUrlAsync);
            BackCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoBack());
            ForwardCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoForward());
            ReloadCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.Reload());
            OpenDevToolsCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.OpenDevTools());
            AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
            OpenHistoryCommand = ReactiveCommand.Create(OpenHistoryWindow);
            AddBookmarkCommand = ReactiveCommand.Create(AddBookmark);
            RefreshSettingsCommand = ReactiveCommand.CreateFromTask(RefreshSettingsAsync);
            NavigateUpCommand = ReactiveCommand.Create(NavigateUp);
            NavigateDownCommand = ReactiveCommand.Create(NavigateDown);
        }

        /// <summary>
        /// Raises property changed events for the commands.
        /// </summary>
        private void UpdateCommands()
        {
            this.RaisePropertyChanged(nameof(GoCommand));
            this.RaisePropertyChanged(nameof(BackCommand));
            this.RaisePropertyChanged(nameof(ForwardCommand));
            this.RaisePropertyChanged(nameof(ReloadCommand));
        }

        /// <summary>
        /// Refreshes the settings by reloading them from the configuration manager.
        /// </summary>
        private async Task RefreshSettingsAsync()
        {
            await _configManager.RefreshSettingsAsync();
            await InitializeKeyBindings();

            string homeUrl = await _configManager.GetHomeUrlAsync();

            foreach (var tab in Tabs)
            {
                if (string.IsNullOrEmpty(tab.Url) || tab.Url == homeUrl)
                {
                    await tab.NavigateAsync(homeUrl);
                }
            }
        }

        /// <summary>
        /// Initializes key bindings from the configuration and sets up commands.
        /// </summary>
        private async Task InitializeKeyBindings()
        {
            var keyBindings = await _configManager.GetKeyBindingsAsync();

            DynamicKeyBindings.Clear();

            foreach (var kvp in keyBindings)
            {
                AddKeyBinding(kvp.Value, kvp.Key switch {
                    "NavigateUp" => NavigateUpCommand,
                    "NavigateDown" => NavigateDownCommand,
                    "AddBookmark" => AddBookmarkCommand,
                    "OpenHistory" => OpenHistoryCommand,
                    "Go" => GoCommand,
                    "Back" => BackCommand,
                    "Forward" => ForwardCommand,
                    "Reload" => ReloadCommand,
                    _ => null
                });
                
                switch (kvp.Key)
                {
                    case "NavigateUp":
                        NavigateUpGesture = kvp.Value;
                        break;
                    case "NavigateDown":
                        NavigateDownGesture = kvp.Value;
                        break;
                    case "AddBookmark":
                        AddBookmarkGesture = kvp.Value;
                        break;
                    case "OpenHistory":
                        OpenHistoryGesture = kvp.Value;
                        break;
                    case "Go":
                        GoGesture = kvp.Value;
                        break;
                    case "Back":
                        BackGesture = kvp.Value;
                        break;
                    case "Forward":
                        ForwardGesture = kvp.Value;
                        break;
                    case "Reload":
                        ReloadGesture = kvp.Value;
                        break;
                }
            }

            this.RaisePropertyChanged(nameof(NavigateUpGesture));
            this.RaisePropertyChanged(nameof(NavigateDownGesture));
            this.RaisePropertyChanged(nameof(AddBookmarkGesture));
            this.RaisePropertyChanged(nameof(OpenHistoryGesture));
            this.RaisePropertyChanged(nameof(GoGesture));
            this.RaisePropertyChanged(nameof(BackGesture));
            this.RaisePropertyChanged(nameof(ForwardGesture));
            this.RaisePropertyChanged(nameof(ReloadGesture));
        }

        /// <summary>
        /// Adds a key binding to the dynamic key bindings collection.
        /// </summary>
        /// <param name="gesture">The key gesture.</param>
        /// <param name="command">The command to execute.</param>
        private void AddKeyBinding(KeyGesture gesture, ICommand? command)
        {
            if (command == null) return;

            var keyBinding = new KeyBinding
            {
                Gesture = gesture,
                Command = command
            };

            DynamicKeyBindings.Add(keyBinding);
        }

        /// <summary>
        /// Adds a new browser tab.
        /// </summary>
        private void AddNewTab()
        {
            var newTab = new BrowserTabViewModel(this);
            Tabs.Add(newTab);
            newTab.NavigateAsync(_configManager.GetHomeUrlAsync().Result).ConfigureAwait(false);
            SelectedTab = newTab;
        }

        /// <summary>
        /// Navigates to the URL entered in the address bar.
        /// </summary>
        private async Task GoToUrlAsync()
        {
            if (string.IsNullOrWhiteSpace(EditableUrl)) return;
            await SelectedTab?.NavigateAsync(EditableUrl);
        }

        /// <summary>
        /// Updates the editable URL with the selected tab's URL.
        /// </summary>
        private void UpdateEditableUrl()
        {
            EditableUrl = SelectedTab?.Url ?? string.Empty;
        }

        /// <summary>
        /// Handles URL change events for the selected tab.
        /// </summary>
        private void OnSelectedTabUrlChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(BrowserTabViewModel.Url)) return;
            UpdateEditableUrl();
            _ = _historyManager.AddToHistoryAsync(SelectedTab?.Url ?? string.Empty);
        }

        /// <summary>
        /// Handles title change events for the selected tab.
        /// </summary>
        private void OnSelectedTabTitleChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BrowserTabViewModel.Header))
            {
                this.RaisePropertyChanged(nameof(Tabs));
            }
        }

        /// <summary>
        /// Navigates to the previous tab in the list.
        /// </summary>
        private void NavigateUp()
        {
            if (Tabs.Count == 0) return;
            var currentIndex = Tabs.IndexOf(SelectedTab);
            var previousIndex = (currentIndex - 1 + Tabs.Count) % Tabs.Count;
            SelectedTab = Tabs[previousIndex];
        }

        /// <summary>
        /// Navigates to the next tab in the list.
        /// </summary>
        private void NavigateDown()
        {
            if (Tabs.Count == 0) return;
            var currentIndex = Tabs.IndexOf(SelectedTab);
            var nextIndex = (currentIndex + 1) % Tabs.Count;
            SelectedTab = Tabs[nextIndex];
        }

        /// <summary>
        /// Loads the bookmarks from the bookmark manager.
        /// </summary>
        public async Task LoadBookmarksAsync()
        {
            var bookmarks = await _bookmarkManager.GetBookmarksAsync();
            Bookmarks.Clear();
            foreach (var (url, title) in bookmarks)
            {
                Bookmarks.Add(new BookmarkItemViewModel(url, title, _bookmarkManager, this));
            }
        }

        /// <summary>
        /// Adds the current URL as a bookmark.
        /// </summary>
        private async void AddBookmark()
        {
            if (string.IsNullOrEmpty(SelectedTab?.Url) || string.IsNullOrEmpty(SelectedTab?.Header)) return;

            await _bookmarkManager.AddBookmarkAsync(SelectedTab.Url, SelectedTab.Header);
            Bookmarks.Add(new BookmarkItemViewModel(SelectedTab.Url, SelectedTab.Header, _bookmarkManager, this));
        }

        /// <summary>
        /// Closes the specified tab.
        /// </summary>
        /// <param name="tab">The tab to close.</param>
        public void CloseTab(BrowserTabViewModel tab)
        {
            if (SelectedTab == tab)
            {
                var currentIndex = Tabs.IndexOf(tab);
                if (Tabs.Count <= 1)
                    AddNewTab();
                SelectedTab = Tabs[(currentIndex - 1 + Tabs.Count) % Tabs.Count];
            }
            Tabs.Remove(tab);
        }

        /// <summary>
        /// Saves the current session.
        /// </summary>
        public async Task SaveSessionAsync()
        {
            await _sessionManager.SaveSessionAsync(Tabs);
        }

        /// <summary>
        /// Loads the session from the session manager.
        /// </summary>
        private async Task LoadSessionAsync()
        {
            var tabs = await _sessionManager.LoadSessionAsync(this);
            foreach (var tab in tabs)
            {
                Tabs.Add(tab);
                await tab.NavigateAsync(tab.Url);
            }

            if (Tabs.Count > 0)
            {
                SelectedTab = Tabs[0];
                UpdateEditableUrl();
            }
        }

        /// <summary>
        /// Opens the history window.
        /// </summary>
        private void OpenHistoryWindow()
        {
            var historyWindow = new HistoryWindow(_historyManager);
            historyWindow.Show();
        }
    }
}
