using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Linq;


namespace Ailurus.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        private BrowserTabViewModel _selectedTab;
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }

        public ReactiveCommand<Unit, Unit> CloseTabCommand { get; }

        public MainWindowViewModel()
        {
            // Initialize with a default tab
            var initialTab = new BrowserTabViewModel { Header = "New Tab" };
            Tabs.Add(initialTab);
            SelectedTab = initialTab;

            CloseTabCommand = ReactiveCommand.Create(() => 
            {
                if (SelectedTab != null)
                {
                    Tabs.Remove(SelectedTab);
                    SelectedTab = Tabs.FirstOrDefault();
                }
            });
        }

        public void AddNewTab()
        {
            var newTab = new BrowserTabViewModel { Header = "New Tab" };
            Tabs.Add(newTab);
            SelectedTab = newTab;
        }
    }
}
