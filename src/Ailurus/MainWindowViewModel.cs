using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Ailurus.ViewModels
{
    public class BrowserTabViewModel : ReactiveObject
    {
        private string _header = string.Empty;
        private object _content = new object();

        public string Header
        {
            get => _header;
            set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        public object Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

        public BrowserTabViewModel()
        {
            CloseTabCommand = ReactiveCommand.Create(() => { /* Close tab logic */ });
        }

        public void Navigate(string url)
        {
            if (_content is CefGlueBrowserControl browserControl)
            {
                browserControl.Navigate(url);
            }
        }
    }

    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<BrowserTabViewModel> Tabs { get; set; } = new ObservableCollection<BrowserTabViewModel>();

        private BrowserTabViewModel _selectedTab = new BrowserTabViewModel();
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }

        public ReactiveCommand<Unit, Unit> AddNewTabCommand { get; }

        public MainWindowViewModel()
        {
            AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
            AddNewTab(); // Start with one tab open
        }

        private void AddNewTab()
        {
            var newTab = new BrowserTabViewModel { Header = "New Tab", Content = new CefGlueBrowserControl() };
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }
    }
}