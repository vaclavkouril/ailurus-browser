using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ailurus.ViewModels;
using Xilium.CefGlue;

namespace Ailurus
{
    public class SessionManager : ISessionManager
    {
        private const string SessionFilePath = "session.json";
        private const string CookiesFilePath = "cookies.json";

        public async Task SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs)
        {
            var sessionData = tabs.Select(tab => new TabSessionData
            {
                Url = tab.BrowserControl.CurrentUrl, // Use the actual URL from the control
                Title = tab.Header
            }).ToList();

            var json = JsonSerializer.Serialize(sessionData);
            await File.WriteAllTextAsync(SessionFilePath, json);

            await SaveCookiesAsync();
        }

        public async Task<IEnumerable<BrowserTabViewModel>> LoadSessionAsync(MainWindowViewModel mainViewModel)
        {
            if (!File.Exists(SessionFilePath))
                return Enumerable.Empty<BrowserTabViewModel>();

            var json = await File.ReadAllTextAsync(SessionFilePath);
            var sessionData = JsonSerializer.Deserialize<List<TabSessionData>>(json) ?? new List<TabSessionData>();

            await LoadCookiesAsync();

            return sessionData.Select(data =>
            {
                var tab = new BrowserTabViewModel(mainViewModel)
                {
                    Url = data.Url
                };
                tab.NavigateAsync(data.Url).ConfigureAwait(false);
                return tab;
            }).ToList();
        }

        private async Task SaveCookiesAsync()
        {
            var cookiesManager = CefCookieManager.GetGlobal(null);
            var cookiesList = new List<CookieData>();

            await Task.Run(() =>
            {
                var visitor = new CookieVisitor(cookiesList);
                cookiesManager.VisitAllCookies(visitor);
                visitor.WaitForCompletion();
            });

            var json = JsonSerializer.Serialize(cookiesList);
            await File.WriteAllTextAsync(CookiesFilePath, json);
        }

        private async Task LoadCookiesAsync()
        {
            if (!File.Exists(CookiesFilePath))
                return;

            var json = await File.ReadAllTextAsync(CookiesFilePath);
            var cookiesList = JsonSerializer.Deserialize<List<CookieData>>(json) ?? new List<CookieData>();

            var cookiesManager = CefCookieManager.GetGlobal(null);

            foreach (var cookie in cookiesList)
            {
                var cefCookie = new CefCookie
                {
                    Domain = cookie.Domain,
                    Name = cookie.Name,
                    Value = cookie.Value,
                    Path = cookie.Path,
                    Secure = cookie.Secure,
                    HttpOnly = cookie.HttpOnly,
                    Expires = cookie.Expires.HasValue ? DateTimeToCefBaseTime(cookie.Expires.Value) : (CefBaseTime?)null
                };

                // Set the cookie back to the manager
                cookiesManager.SetCookie(cookie.Url ?? string.Empty, cefCookie, null);
            }
        }

        private class TabSessionData
        {
            public string Url { get; set; } = string.Empty;
            public string Title { get; set; } = string.Empty;
        }

        private class CookieData
        {
            public string Name { get; init; } = string.Empty;
            public string Value { get; init; } = string.Empty;
            public string Domain { get; init; } = string.Empty;
            public string Path { get; init; } = string.Empty;
            public string Url { get; init; } = string.Empty;
            public DateTime? Expires { get; init; }
            public bool Secure { get; init; } = false;
            public bool HttpOnly { get; init; } = false;
        }

        private class CookieVisitor : CefCookieVisitor
        {
            private readonly List<CookieData> _cookiesList;
            private readonly TaskCompletionSource<bool> _completionSource = new();

            public CookieVisitor(List<CookieData> cookiesList)
            {
                _cookiesList = cookiesList;
            }

            protected override bool Visit(CefCookie cookie, int count, int total, out bool deleteCookie)
            {
                deleteCookie = false;

                _cookiesList.Add(new CookieData
                {
                    Name = cookie.Name ?? string.Empty,
                    Value = cookie.Value ?? string.Empty,
                    Domain = cookie.Domain ?? string.Empty,
                    Path = cookie.Path ?? string.Empty,
                    Url = $"{cookie.Domain}{cookie.Path}",
                    Expires = cookie.Expires.HasValue ? CefBaseTimeToDateTime(cookie.Expires.Value) : null,
                    Secure = cookie.Secure,
                    HttpOnly = cookie.HttpOnly
                });

                if (count == total - 1)
                {
                    _completionSource.SetResult(true);
                }

                return true;
            }

            public void WaitForCompletion()
            {
                _completionSource.Task.Wait();
            }
        }
        
        private static CefBaseTime DateTimeToCefBaseTime(DateTime dateTime)
        {
            var cefTime = new CefTime(dateTime.ToUniversalTime());
            if (CefBaseTime.FromUtcExploded(in cefTime, out CefBaseTime cefBaseTime))
            {
                return cefBaseTime;
            }
            throw new InvalidOperationException("Failed to convert DateTime to CefBaseTime.");
        }
        
        private static DateTime CefBaseTimeToDateTime(CefBaseTime cefBaseTime)
        {
            if (cefBaseTime.UtcExplode(out CefTime cefTime))
            {
                return cefTime.ToDateTime();
            }
            throw new InvalidOperationException("Failed to convert CefBaseTime to DateTime.");
        }
    }
}
