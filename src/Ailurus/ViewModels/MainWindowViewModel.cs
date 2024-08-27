using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly ISessionManager _sessionManager;

        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        private BrowserTabViewModel _selectedTab;
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                if (_selectedTab != value)
                {
                    _selectedTab?.SetIsSelected(false);
                    this.RaiseAndSetIfChanged(ref _selectedTab, value);
                    _selectedTab?.SetIsSelected(true);
                    this.RaisePropertyChanged(nameof(BrowserContent));

                    UpdateCommands();
                }
            }
        }

        public AvaloniaCefBrowser? BrowserContent => SelectedTab?.Browser;

        public string? Url
        {
            get => _selectedTab?.Url;
            set => _selectedTab.Url = value;
        }

        public ReactiveCommand<Unit, Unit> AddNewTabCommand { get; }
        public ReactiveCommand<Unit, Unit> GoCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> BackCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ForwardCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> OpenDevToolsCommand { get; private set; }

        public MainWindowViewModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;

            AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
            InitializeCommands();
            LoadSessionAsync().ConfigureAwait(false);
        }

        private void InitializeCommands()
        {
            GoCommand = ReactiveCommand.CreateFromTask(GoToUrlAsync);
            BackCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoBack());
            ForwardCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoForward());
            ReloadCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.Reload());
            OpenDevToolsCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.OpenDevTools());
        }

        private void UpdateCommands()
        {
            GoCommand = ReactiveCommand.CreateFromTask(GoToUrlAsync);
            BackCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoBack());
            ForwardCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.GoForward());
            ReloadCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.Reload());
            OpenDevToolsCommand = ReactiveCommand.Create(() => SelectedTab?.BrowserControl.OpenDevTools());

            this.RaisePropertyChanged(nameof(GoCommand));
            this.RaisePropertyChanged(nameof(BackCommand));
            this.RaisePropertyChanged(nameof(ForwardCommand));
            this.RaisePropertyChanged(nameof(ReloadCommand));
            this.RaisePropertyChanged(nameof(OpenDevToolsCommand));
        }

        private void AddNewTab()
        {
            var newTab = new BrowserTabViewModel(this);
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        private async Task GoToUrlAsync()
        {
            if (SelectedTab != null && !string.IsNullOrWhiteSpace(Url))
            {
                await SelectedTab.NavigateAsync(Url);
            }
        }

        public void CloseTab(BrowserTabViewModel tab)
        {
            if (SelectedTab == tab)
            {
                var currentIndex = Tabs.IndexOf(tab);
                SelectedTab = Tabs.Count > 1 ? Tabs[(currentIndex - 1 + Tabs.Count) % Tabs.Count] : null;
            }
            Tabs.Remove(tab);
        }

        public async Task SaveSessionAsync()
        {
            await _sessionManager.SaveSessionAsync(Tabs);
            // Ensure the tab navigates to its stored URL
            
        }

        private async Task LoadSessionAsync()
        {
            var tabs = await _sessionManager.LoadSessionAsync(this);
            foreach (var tab in tabs)
            {
                Tabs.Add(tab);
                // Loading of the tabs from stored url
                tab.NavigateAsync(tab.Url);
            }

            if (Tabs.Count > 0)
            {
                SelectedTab = Tabs[0];
            }
        }
    }
}
