using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Ailurus.ViewModels
{
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header = "New Tab";
        private string _url = "https://www.google.com";
        private bool _isLoading;
        private CefGlueBrowserControl _content;
        private bool _isSelected;

        public string Header
        {
            get => _header;
            set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
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

        public CefGlueBrowserControl Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectTabCommand { get; }

        public BrowserTabViewModel(MainWindowViewModel mainWindowViewModel)
        {
            _content = new CefGlueBrowserControl();
            _content.Initialize(this);
            _content.NavigateAsync(_url);

            CloseTabCommand = ReactiveCommand.Create(() => mainWindowViewModel.CloseTab(this));
            SelectTabCommand = ReactiveCommand.Create(() =>
            {
                mainWindowViewModel.SelectedTab = this;
                return Unit.Default;
            });
        }

        public async Task NavigateAsync(string url)
        {
            await _content.NavigateAsync(url);
        }
    }
}