using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using Avalonia.Controls;

namespace Ailurus
{
    public class TabController
    {
        private readonly TabControl _tabControl;
        public ObservableCollection<BrowserTabViewModel> Tabs { get; } = new ObservableCollection<BrowserTabViewModel>();

        public TabController(TabControl tabControl)
        {
            _tabControl = tabControl;
            _tabControl.ItemsSource = Tabs;
        }

        public void AddNewTab(string url)
        {
            var newTab = new BrowserTabViewModel
            {
                Header = "New Tab",
                Content = new CefGlueBrowserControl(),
                CloseTabCommand = ReactiveCommand.Create<BrowserTabViewModel>(tab =>
                {
                    Tabs.Remove(tab);
                })
            };

            Tabs.Add(newTab);
            _tabControl.SelectedItem = newTab;
            newTab.Navigate(url);
        }

        public void NavigateCurrentTab(string url)
        {
            if (_tabControl.SelectedItem is BrowserTabViewModel selectedTab)
            {
                selectedTab.Navigate(url);
            }
        }
    }
}
