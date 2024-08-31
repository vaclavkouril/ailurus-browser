using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly ISessionManager _sessionManager;
    private readonly IHistoryManager _historyManager;
    private readonly IBookmarkManager _bookmarkManager;

    private readonly string _defaultUrl = "http://www.google.com";

    public ObservableCollection<BrowserTabViewModel> Tabs { get; } = [];
    public ObservableCollection<BookmarkItemViewModel> Bookmarks { get; } = [];

    private BrowserTabViewModel _selectedTab;
    public BrowserTabViewModel SelectedTab
    {
        get => _selectedTab;
        set
        {
            if (_selectedTab == value) return;
                
            _selectedTab.PropertyChanged -= OnSelectedTabUrlChanged;
            _selectedTab.PropertyChanged -= OnSelectedTabTitleChanged;
                    

            _selectedTab.SetIsSelected(false);
            this.RaiseAndSetIfChanged(ref _selectedTab, value);
            _selectedTab.SetIsSelected(true);

                
            _selectedTab.PropertyChanged += OnSelectedTabUrlChanged;
            _selectedTab.PropertyChanged += OnSelectedTabTitleChanged;
            UpdateEditableUrl();
                

            this.RaisePropertyChanged(nameof(BrowserContent));
            UpdateCommands();
        }
    }

    public AvaloniaCefBrowser BrowserContent => SelectedTab.Browser;

    private string _editableUrl;
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
    public ReactiveCommand<Unit, Unit> OpenHistoryCommand { get; private set;}
    public ReactiveCommand<Unit, Unit> AddBookmarkCommand { get; private set;}
    public ReactiveCommand<Unit, Unit> LaunchAnonymousModeCommand { get; set; }

    public MainWindowViewModel(ISessionManager sessionManager, IHistoryManager historyManager, IBookmarkManager bookmarkManager)
    {
        _sessionManager = sessionManager;
        _historyManager = historyManager;
        _bookmarkManager = bookmarkManager;

            

        InitializeCommands();
        LoadSessionAsync().ConfigureAwait(false);
        LoadBookmarksAsync().ConfigureAwait(false);
    }

    private void InitializeCommands()
    {
        GoCommand = ReactiveCommand.CreateFromTask(GoToUrlAsync);
        BackCommand = ReactiveCommand.Create(() => SelectedTab.BrowserControl.GoBack());
        ForwardCommand = ReactiveCommand.Create(() => SelectedTab.BrowserControl.GoForward());
        ReloadCommand = ReactiveCommand.Create(() => SelectedTab.BrowserControl.Reload());
        OpenDevToolsCommand = ReactiveCommand.Create(() => SelectedTab.BrowserControl.OpenDevTools());
        AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
        OpenHistoryCommand = ReactiveCommand.Create(OpenHistoryWindow);
        AddBookmarkCommand = ReactiveCommand.Create(AddBookmark);
    }

    private void UpdateCommands()
    {
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
        newTab.NavigateAsync(_defaultUrl).ConfigureAwait(false);
    }

    private async Task GoToUrlAsync()
    {
        if (string.IsNullOrWhiteSpace(EditableUrl)) await Task.CompletedTask;
        await SelectedTab.NavigateAsync(EditableUrl);
    }

    private void UpdateEditableUrl()
    {
        EditableUrl = SelectedTab.Url;
    }
        
    private void OnSelectedTabUrlChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(BrowserTabViewModel.Url)) return;
        UpdateEditableUrl();
        _historyManager.AddToHistoryAsync(SelectedTab.Url);
    }

    private void OnSelectedTabTitleChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(BrowserTabViewModel.Header))
        {
            this.RaisePropertyChanged(nameof(Tabs));
        }
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
        if (string.IsNullOrEmpty(SelectedTab.Url) || string.IsNullOrEmpty(SelectedTab.Header)) return;
            
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