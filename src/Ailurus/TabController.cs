using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace Ailurus
{
    public class TabController
    {
        private readonly TabControl _tabControl;
        private readonly ObservableCollection<BrowserTab> _tabs;

        public TabController(TabControl tabControl)
        {
            _tabControl = tabControl;
            _tabs = new ObservableCollection<BrowserTab>();
            _tabControl.Items = _tabs;
        }

        public void AddTab(string url)
        {
            var newTab = new BrowserTab(url);
            _tabs.Add(newTab);
            _tabControl.SelectedItem = newTab; // Optionally select the new tab
        }

        public void CloseTab(BrowserTab tab)
        {
            _tabs.Remove(tab);
        }

        public void NavigateCurrentTab(string url)
        {
            if (_tabControl.SelectedItem is BrowserTab currentTab)
            {
                currentTab.Navigate(url);
            }
        }

        public BrowserTab GetCurrentTab()
        {
            return _tabControl.SelectedItem as BrowserTab;
        }
    }
}
