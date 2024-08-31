using System.Reactive;
using ReactiveUI;
using System.Threading.Tasks;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels;

public class BrowserTabViewModel : ReactiveObject
{
    private string _header = "New Tab";
    private string _url = "https://www.google.com";
    private bool _isLoading;
    private bool _isSelected;

    public string Header
    {
        get => _header;
        private set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    public string Url
    {
        get => BrowserControl.CurrentUrl;
        set
        {
            // Redundant to set value that is already assigned
            if (_url == value) return;
            
            this.RaiseAndSetIfChanged(ref _url, value);
            NavigateAsync(value).ConfigureAwait(false);
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => this.RaiseAndSetIfChanged(ref _isLoading, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public AvaloniaCefBrowser Browser { get; }

    public CefGlueBrowserControl BrowserControl { get; }

    public BrowserTabViewModel(MainWindowViewModel mainWindowViewModel)
    {
        Browser = new AvaloniaCefBrowser();
        BrowserControl = new CefGlueBrowserControl(Browser);
            
        Browser.AddressChanged += OnBrowserAddressChanged;
        Browser.TitleChanged += OnBrowserTitleChanged;

        CloseTabCommand = ReactiveCommand.Create(() => mainWindowViewModel.CloseTab(this));
        SelectTabCommand = ReactiveCommand.Create(() =>
        {
            mainWindowViewModel.SelectedTab = this;
            return Unit.Default;
        });
    }

    private void OnBrowserAddressChanged(object sender, string e)
    {
        this.RaisePropertyChanged(nameof(Url));
    }

    private void OnBrowserTitleChanged(object sender, string newTitle)
    {
        Header = BrowserControl.Title;
    }

    public async Task NavigateAsync(string url)
    {
        if (!string.IsNullOrWhiteSpace(url) && BrowserControl.CurrentUrl != url)
        {
            await BrowserControl.NavigateAsync(url);
        }
    }

    public void SetIsSelected(bool isSelected)
    {
        IsSelected = isSelected;
    }

    public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectTabCommand { get; }
}