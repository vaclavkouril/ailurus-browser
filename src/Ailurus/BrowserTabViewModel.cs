using ReactiveUI;
using System.Reactive;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    /// <summary>
    /// ViewModel representing a single browser tab in the application.
    /// </summary>
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header = "New Tab";
        private string _url = "DEBUG URL";
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
        /// TODO: Command that closes the browser tab.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserTabViewModel"/> class.
        /// </summary>
        /// <param name="mainWindowViewModel">The MainWindowViewModel that manages this tab.</param>
        public BrowserTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _content = new CefGlueBrowserControl();
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
}
