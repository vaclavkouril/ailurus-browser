using System;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Xilium.CefGlue.Avalonia;

namespace Ailurus.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        private BrowserTabViewModel _selectedTab;
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedTab, value);
                foreach (var tab in Tabs)
                {
                    tab.IsSelected = tab == _selectedTab;
                }
            }
        }

        private string _url;
        public string Url
        {
            get => _url;
            set => this.RaiseAndSetIfChanged(ref _url, value);
        }

        public ReactiveCommand<Unit, Unit> AddNewTabCommand { get; }
        public ReactiveCommand<Unit, Unit> GoCommand { get; }

        public MainWindowViewModel()
        {
            AddNewTabCommand = ReactiveCommand.Create(AddNewTab);
            GoCommand = ReactiveCommand.CreateFromTask(async () => await GoToUrlAsync());
            Browser = new AvaloniaCefBrowser();
            Console.WriteLine(Browser.IsVisible);
            Browser.Address = "https://www.google.com";
        }
        public AvaloniaCefBrowser Browser { get; set; }
        private void AddNewTab()
        {
            var newTab = new BrowserTabViewModel(this) { Header = "New Tab" };
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }

        private async Task GoToUrlAsync()
        {
            if (SelectedTab != null)
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
    }
}