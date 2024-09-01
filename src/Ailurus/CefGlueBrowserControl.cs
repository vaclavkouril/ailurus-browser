using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Xilium.CefGlue.Avalonia;

namespace Ailurus;

/// <summary>
/// Represents a browser control based on CefGlue, providing functionalities to navigate, reload, and manage the browser's state.
/// </summary>
public class CefGlueBrowserControl : Control
{
    private readonly AvaloniaCefBrowser _browser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CefGlueBrowserControl"/> class with the specified browser instance.
    /// </summary>
    /// <param name="browser">The <see cref="AvaloniaCefBrowser"/> instance to use within this control.</param>
    /// <exception cref="ArgumentNullException">Thrown when the provided browser instance is null.</exception>
    public CefGlueBrowserControl(AvaloniaCefBrowser browser)
    {
        _browser = browser ?? throw new ArgumentNullException(nameof(browser));
        InitializeControl();
    }

    /// <summary>
    /// Initializes the control by adding the browser instance to the logical children of the control.
    /// </summary>
    private void InitializeControl()
    {
        LogicalChildren.Add(_browser);
    }

    /// <summary>
    /// Navigates the browser to the specified URL asynchronously.
    /// </summary>
    /// <param name="url">The URL to navigate to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task NavigateAsync(string url)
    {
        string normalizedUrl = NormalizeUrl(url);
        _browser.Address = normalizedUrl;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Reloads the current page in the browser.
    /// </summary>
    public void Reload()
    {
        _browser.Reload();
    }

    /// <summary>
    /// Navigates the browser back to the previous page, if possible.
    /// </summary>
    public void GoBack()
    {
        if (_browser.CanGoBack) _browser.GoBack();
    }

    /// <summary>
    /// Navigates the browser forward to the next page, if possible.
    /// </summary>
    public void GoForward()
    {
        if (_browser.CanGoForward) _browser.GoForward();
    }

    /// <summary>
    /// Opens the developer tools for the browser.
    /// </summary>
    public void OpenDevTools()
    {
        _browser.ShowDeveloperTools();
    }

    /// <summary>
    /// Gets or sets the current URL of the browser.
    /// </summary>
    public string CurrentUrl
    {
        get => _browser.Address;
        set => _browser.Address = value;
    }

    /// <summary>
    /// Gets the title of the current page loaded in the browser.
    /// </summary>
    public string Title => _browser.Title;

    /// <summary>
    /// Normalizes a URL by ensuring it is well-formed and prepending appropriate schemes if necessary.
    /// </summary>
    /// <param name="url">The URL to normalize.</param>
    /// <returns>The normalized URL.</returns>
    private string NormalizeUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return "about:blank";
        }

        url = url.Trim();

        return url switch
        {
            _ when url.StartsWith("about:", StringComparison.OrdinalIgnoreCase) => url,
            _ when url.StartsWith("file://", StringComparison.OrdinalIgnoreCase) => url,
            _ when Uri.IsWellFormedUriString(url, UriKind.Absolute) => url,
            _ when IsLikelyHostOrIp(url) => $"https://{url}",
            _ when url.Contains('.') && !url.Contains(' ') => $"https://{url}",
            _ => $"https://www.google.com/search?q={Uri.EscapeDataString(url)}"
        };
    }

    /// <summary>
    /// Determines if the given string is likely a host name or IP address.
    /// </summary>
    /// <param name="url">The string to check.</param>
    /// <returns><c>true</c> if the string is likely a host name or IP address; otherwise, <c>false</c>.</returns>
    private bool IsLikelyHostOrIp(string url)
    {
        if (IsIpAddress(url))
        {
            return true;
        }

        if (Uri.CheckHostName(url) == UriHostNameType.Dns)
        {
            return url.Contains('.') && !url.StartsWith('.') && !url.EndsWith('.');
        }

        return false;
    }

    /// <summary>
    /// Checks if the provided string is a valid IP address.
    /// </summary>
    /// <param name="url">The string to check.</param>
    /// <returns><c>true</c> if the string is a valid IP address; otherwise, <c>false</c>.</returns>
    private static bool IsIpAddress(string url) => System.Net.IPAddress.TryParse(url, out _);
}