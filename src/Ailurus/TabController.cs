using Ailurus.ViewModels;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Linq;

namespace Ailurus
{
    public class TabController
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public TabController(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public void AddNewTab(string initialUrl = "https://www.google.com")
        {
            var browserControl = new CefGlueBrowserControl(); 
            var newTab = new BrowserTabViewModel(_mainWindowViewModel)
            {
                Header = "New Tab",
                Content = browserControl
            };

            // Initialize the CefGlueBrowserControl with the BrowserTabViewModel
            browserControl.Initialize(newTab);

            _mainWindowViewModel.Tabs.Add(newTab);
            _mainWindowViewModel.SelectedTab = newTab;

            // Navigate to the initial URL if provided
            newTab.Navigate(initialUrl);
        }
        public void CloseTab(BrowserTabViewModel tab)
        {
            if (_mainWindowViewModel.Tabs.Contains(tab))
            {
                _mainWindowViewModel.Tabs.Remove(tab);

                // Set the selected tab to the next available tab
                if (_mainWindowViewModel.Tabs.Any())
                {
                    _mainWindowViewModel.SelectedTab = _mainWindowViewModel.Tabs.FirstOrDefault();
                }
                else
                {
                    // If no tabs left, maybe open a new tab or handle accordingly
                    AddNewTab();
                }
            }
        }

        public void NavigateCurrentTab(string url)
        {
            _mainWindowViewModel.SelectedTab?.Navigate(url);
        }

        public void GoBack()
        {
            if (_mainWindowViewModel.SelectedTab?.Content is CefGlueBrowserControl browser)
            {
                browser.GoBack();
            }
        }

        public void GoForward()
        {
            if (_mainWindowViewModel.SelectedTab?.Content is CefGlueBrowserControl browser)
            {
                browser.GoForward();
            }
        }

        public void Reload()
        {
            if (_mainWindowViewModel.SelectedTab?.Content is CefGlueBrowserControl browser)
            {
                browser.Reload();
            }
        }
    }
}
