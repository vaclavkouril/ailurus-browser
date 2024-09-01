using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ailurus.ViewModels;
using Xilium.CefGlue;

namespace Ailurus;

/// <summary>
/// Manages the session for the browser, including saving and loading tabs and cookies.
/// </summary>
public class SessionManager : ISessionManager
{
    private const string SessionFilePath = "session.json";
    private const string CookiesFilePath = "cookies.json";

    /// <summary>
    /// Saves the current session by persisting open tabs and cookies.
    /// </summary>
    /// <param name="tabs">The collection of open tabs.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
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

    /// <summary>
    /// Loads a previously saved session, restoring open tabs and cookies.
    /// </summary>
    /// <param name="mainViewModel">The view model for the main window.</param>
    /// <returns>A <see cref="Task"/> containing the restored tabs.</returns>
    public async Task<IEnumerable<BrowserTabViewModel>> LoadSessionAsync(MainWindowViewModel mainViewModel)
    {
        if (!File.Exists(SessionFilePath))
            return [];

        var json = await File.ReadAllTextAsync(SessionFilePath);
        var sessionData = JsonSerializer.Deserialize<List<TabSessionData>>(json) ?? [];

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

    /// <summary>
    /// Saves browser cookies to a file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous save operation.</returns>
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

    /// <summary>
    /// Loads browser cookies from a file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous load operation.</returns>
    private async Task LoadCookiesAsync()
    {
        if (!File.Exists(CookiesFilePath))
            return;

        var json = await File.ReadAllTextAsync(CookiesFilePath);
        var cookiesList = JsonSerializer.Deserialize<List<CookieData>>(json) ?? [];

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
                Expires = cookie.Expires.HasValue ? DateTimeToCefBaseTime(cookie.Expires.Value) : null
            };

            // Set the cookie back to the manager
            cookiesManager.SetCookie(cookie.Url ?? string.Empty, cefCookie, null);
        }
    }

    /// <summary>
    /// Represents the data required to save a tab session.
    /// </summary>
    private class TabSessionData
    {
        public string Url { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents the data required to save a browser cookie.
    /// </summary>
    private class CookieData
    {
        public string Name { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        public string Domain { get; init; } = string.Empty;
        public string Path { get; init; } = string.Empty;
        public string Url { get; init; } = string.Empty;
        public DateTime? Expires { get; init; }
        public bool Secure { get; init; }
        public bool HttpOnly { get; init; }
    }

    /// <summary>
    /// Visitor class for handling cookies during the saving process.
    /// </summary>
    private class CookieVisitor(List<CookieData> cookiesList) : CefCookieVisitor
    {
        private readonly TaskCompletionSource<bool> _completionSource = new();

        /// <summary>
        /// Visits each cookie and adds it to the list of cookies to be saved.
        /// </summary>
        /// <param name="cookie">The current cookie being visited.</param>
        /// <param name="count">The current cookie's index.</param>
        /// <param name="total">The total number of cookies.</param>
        /// <param name="deleteCookie">Indicates whether the cookie should be deleted.</param>
        /// <returns>True to continue visiting cookies; otherwise, false.</returns>
        protected override bool Visit(CefCookie cookie, int count, int total, out bool deleteCookie)
        {
            deleteCookie = false;

            cookiesList.Add(new CookieData
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

        /// <summary>
        /// Waits for the cookie visitation to complete.
        /// </summary>
        public void WaitForCompletion()
        {
            _completionSource.Task.Wait();
        }
    }
        
    /// <summary>
    /// Converts a <see cref="DateTime"/> to a CEF-specific time format.
    /// </summary>
    /// <param name="dateTime">The <see cref="DateTime"/> to convert.</param>
    /// <returns>The converted <see cref="CefBaseTime"/>.</returns>
    private static CefBaseTime DateTimeToCefBaseTime(DateTime dateTime)
    {
        var cefTime = new CefTime(dateTime.ToUniversalTime());
        if (CefBaseTime.FromUtcExploded(in cefTime, out CefBaseTime cefBaseTime))
        {
            return cefBaseTime;
        }
        throw new InvalidOperationException("Failed to convert DateTime to CefBaseTime.");
    }
        
    /// <summary>
    /// Converts a CEF-specific time format to a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="cefBaseTime">The CEF-specific time format to convert.</param>
    /// <returns>The converted <see cref="DateTime"/>.</returns>
    private static DateTime CefBaseTimeToDateTime(CefBaseTime cefBaseTime)
    {
        if (cefBaseTime.UtcExplode(out CefTime cefTime))
        {
            return cefTime.ToDateTime();
        }
        throw new InvalidOperationException("Failed to convert CefBaseTime to DateTime.");
    }
}