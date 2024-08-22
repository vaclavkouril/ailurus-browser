using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive;

namespace Ailurus
{
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header;
        private CefGlueBrowserControl _content;

        public BrowserTabViewModel()
        {
            _header = string.Empty; // Initialize with an empty string
            _content = new CefGlueBrowserControl(); // Initialize with a new instance of the control
            CloseTabCommand = ReactiveCommand.Create<BrowserTabViewModel, Unit>(_ => Unit.Default); // Initialize the command
        }

        public string Header
        {
            get => _header;
            set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        public CefGlueBrowserControl Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public ReactiveCommand<BrowserTabViewModel, Unit> CloseTabCommand { get; set; }

        public void Navigate(string url)
        {
            Content.Navigate(url);
            Header = url; // Update the tab header with the URL
        }
    }
}
