using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Input;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ISessionManager _sessionManager;
        private readonly IHistoryManager _historyManager;
        private readonly IBookmarkManager _bookmarkManager;
        private readonly IConfigurationManager _configManager;

        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();
        public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = new ObservableCollection<BookmarkItemViewModel>();
        public ObservableCollection<KeyBinding> DynamicKeyBindings { get; } = new ObservableCollection<KeyBinding>();

        private BrowserTabViewModel? _selectedTab;
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

        public AvaloniaCefBrowser? BrowserContent => SelectedTab?.Browser;

        private string _editableUrl = string.Empty;
        public string EditableUrl
        {
            get => _editableUrl;
            set => this.RaiseAndSetIfChanged(ref _editableUrl, value);
        }

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

        // Key Gestures (These can be populated dynamically from the configuration manager)
        public KeyGesture BackGesture { get; private set; }
        public KeyGesture ForwardGesture { get; private set; }
        public KeyGesture ReloadGesture { get; private set; }
        public KeyGesture GoGesture { get; private set; }
        public KeyGesture AddBookmarkGesture { get; private set; }
        public KeyGesture OpenHistoryGesture { get; private set; }
        public KeyGesture NavigateUpGesture { get; private set; }
        public KeyGesture NavigateDownGesture { get; private set; }

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

        private void UpdateCommands()
        {
            this.RaisePropertyChanged(nameof(GoCommand));
            this.RaisePropertyChanged(nameof(BackCommand));
            this.RaisePropertyChanged(nameof(ForwardCommand));
            this.RaisePropertyChanged(nameof(ReloadCommand));
        }

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

                // Setting the gestures dynamically based on configuration
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

        private void AddNewTab()
        {
            var newTab = new BrowserTabViewModel(this);
            Tabs.Add(newTab);
            newTab.NavigateAsync(_configManager.GetHomeUrlAsync().Result).ConfigureAwait(false);
            SelectedTab = newTab;
        }

        private async Task GoToUrlAsync()
        {
            if (string.IsNullOrWhiteSpace(EditableUrl)) return;
            await SelectedTab?.NavigateAsync(EditableUrl);
        }

        private void UpdateEditableUrl()
        {
            EditableUrl = SelectedTab?.Url ?? string.Empty;
        }

        private void OnSelectedTabUrlChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(BrowserTabViewModel.Url)) return;
            UpdateEditableUrl();
            _ = _historyManager.AddToHistoryAsync(SelectedTab?.Url ?? string.Empty);
        }

        private void OnSelectedTabTitleChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BrowserTabViewModel.Header))
            {
                this.RaisePropertyChanged(nameof(Tabs));
            }
        }

        private void NavigateUp()
        {
            if (Tabs.Count == 0) return;
            var currentIndex = Tabs.IndexOf(SelectedTab);
            var previousIndex = (currentIndex - 1 + Tabs.Count) % Tabs.Count;
            SelectedTab = Tabs[previousIndex];
        }

        private void NavigateDown()
        {
            if (Tabs.Count == 0) return;
            var currentIndex = Tabs.IndexOf(SelectedTab);
            var nextIndex = (currentIndex + 1) % Tabs.Count;
            SelectedTab = Tabs[nextIndex];
        }

        public async Task LoadBookmarksAsync()
        {
            var bookmarks = await _bookmarkManager.GetBookmarksAsync();
            Bookmarks.Clear();
            foreach (var (url, title) in bookmarks)
            {
                Bookmarks.Add(new BookmarkItemViewModel(url, title, _bookmarkManager, this));
            }
        }

        private async void AddBookmark()
        {
            if (string.IsNullOrEmpty(SelectedTab?.Url) || string.IsNullOrEmpty(SelectedTab?.Header)) return;

            await _bookmarkManager.AddBookmarkAsync(SelectedTab.Url, SelectedTab.Header);
            Bookmarks.Add(new BookmarkItemViewModel(SelectedTab.Url, SelectedTab.Header, _bookmarkManager, this));
        }

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

        public async Task SaveSessionAsync()
        {
            await _sessionManager.SaveSessionAsync(Tabs);
        }

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

        private void OpenHistoryWindow()
        {
            var historyWindow = new HistoryWindow(_historyManager);
            historyWindow.Show();
        }
    }
}
