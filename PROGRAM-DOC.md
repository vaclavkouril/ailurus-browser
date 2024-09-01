# Ailurus Program Documentation

## Table of Contents

- [Introduction](#introduction)
- [Classes and Interfaces](#classes-and-interfaces)
  - [Ailurus.AnonymousBookmarkManager](#ailurusanonymousbookmarkmanager)
  - [Ailurus.AnonymousHistoryManager](#ailurusanonymoushistorymanager)
  - [Ailurus.AnonymousSessionManager](#ailurusanonymoussessionmanager)
  - [Ailurus.App](#ailurusapp)
  - [Ailurus.BookmarkItem](#ailurusbookmarkitem)
  - [Ailurus.BookmarkManager](#ailurusbookmarkmanager)
  - [Ailurus.CefGlueBrowserControl](#ailuruscefgluebrowsercontrol)
  - [Ailurus.ConfigurationManager](#ailurusconfigurationmanager)
  - [Ailurus.HistoryItem](#ailurushistoryitem)
  - [Ailurus.HistoryManager](#ailurushistorymanager)
  - [Ailurus.IBookmarkManager](#ailurusibookmarkmanager)
  - [Ailurus.IConfigurationManager](#ailurusiconfigurationmanager)
  - [Ailurus.IHistoryManager](#ailurusihistorymanager)
  - [Ailurus.ISessionManager](#ailurusisessionmanager)
  - [Ailurus.MainWindow](#ailurusmainwindow)
  - [Ailurus.Program](#ailurusprogram)
  - [Ailurus.SessionManager](#ailurussessionmanager)
  - [Ailurus.Converters.SelectedTabConverter](#ailurusconvertersselectedtabconverter)
  - [Ailurus.ViewModels](#ailurusviewmodels)
    - [Ailurus.ViewModels.BookmarkItemViewModel](#ailurusviewmodelsbookmarkitemviewmodel)
    - [Ailurus.ViewModels.BrowserTabViewModel](#ailurusviewmodelsbrowsertabviewmodel)
    - [Ailurus.ViewModels.HistoryItemViewModel](#ailurusviewmodelshistoryitemviewmodel)
    - [Ailurus.ViewModels.HistoryWindowViewModel](#ailurusviewmodelshistorywindowviewmodel)
    - [Ailurus.ViewModels.MainWindowViewModel](#ailurusviewmodelsmainwindowviewmodel)
  
## Introduction

Ailurus is a web browser application designed to operate in both anonymous and normal modes. The application includes a wide range of features including bookmark management, session handling, and history tracking. This documentation covers classes, interfaces, and methods available in the Ailurus application.

## Classes and Interfaces

### Ailurus.AnonymousBookmarkManager

Manages bookmarks in anonymous mode, where bookmarks are loaded from a file but not persisted between sessions.

#### Constructor

- **`Ailurus.AnonymousBookmarkManager.#ctor`**
  - Initializes a new instance of the `Ailurus.AnonymousBookmarkManager` class.
  - Automatically loads bookmarks from a file on instantiation.

#### Methods

- **`AddBookmarkAsync(string url, string title)`**
  - Adds a new bookmark asynchronously.
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.
  - Returns: `Task`.

- **`GetBookmarksAsync()`**
  - Retrieves all bookmarks asynchronously.
  - Returns: `Task<IEnumerable<Tuple<string, string>>>`.

- **`RemoveBookmarkAsync(string url)`**
  - Removes a bookmark with the specified URL asynchronously.
  - Parameters:
    - `url`: The URL of the bookmark to remove.
  - Returns: `Task`.

- **`GetSelectedBookmarkAsync()`**
  - Retrieves the currently selected bookmark asynchronously.
  - Returns: `Task<BookmarkItem>`.

- **`SelectBookmarkAsync(string url)`**
  - Selects a bookmark by its URL asynchronously.
  - Parameters:
    - `url`: The URL of the bookmark to select.
  - Returns: `Task`.

- **`LoadBookmarks()`**
  - Loads bookmarks from a file during initialization.
  - The bookmarks are loaded but not saved or modified afterward.

### Ailurus.AnonymousHistoryManager

Manages browser history in anonymous mode, where no history data is persisted or retrieved.

#### Methods

- **`AddToHistoryAsync(string url)`**
  - Adds a URL to the browsing history asynchronously.
  - In anonymous mode, this does nothing.
  - Parameters:
    - `url`: The URL to add to the history.
  - Returns: `Task`.

- **`GetHistoryAsync()`**
  - Retrieves the browsing history asynchronously.
  - In anonymous mode, this returns an empty collection.
  - Returns: `Task<IEnumerable<HistoryItem>>`.

- **`DeleteHistoryAsync()`**
  - Deletes all browsing history asynchronously.
  - In anonymous mode, this does nothing.
  - Returns: `Task`.

### Ailurus.AnonymousSessionManager

A session manager that operates in anonymous mode, meaning no session data is saved or loaded.

#### Methods

- **`SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs)`**
  - Does not save the current session as anonymous mode does not persist session data.
  - Parameters:
    - `tabs`: The collection of `BrowserTabViewModel` instances representing the open tabs.
  - Returns: `Task`.

- **`LoadSessionAsync(MainWindowViewModel window)`**
  - Does not load any session as anonymous mode does not persist session data.
  - Parameters:
    - `window`: The `MainWindowViewModel` where the tabs would be restored.
  - Returns: `Task<IEnumerable<BrowserTabViewModel>>`.

### Ailurus.App

Represents the main application class for Ailurus. It initializes and configures the application, including setting up anonymous mode if specified.

#### Methods

- **`Initialize()`**
  - Initializes the application by loading the XAML resources.

- **`Configure(bool isAnonymousMode)`**
  - Configures the application mode (anonymous or normal).
  - Parameters:
    - `isAnonymousMode`: Indicates whether the application should run in anonymous mode.

- **`OnFrameworkInitializationCompleted()`**
  - Called when the application framework initialization is completed.
  - Sets up the main window and configures session, history, and bookmark managers based on the application mode.

- **`ShutdownApplicationAsync()`**
  - Shuts down the application, including any resources used by the CefGlue browser.
  - Returns: `Task`.

### Ailurus.BookmarkItem

Represents a bookmark item with a URL and a title.

#### Constructor

- **`Ailurus.BookmarkItem.#ctor(string url, string title)`**
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.

### Ailurus.BookmarkManager

Manages bookmarks, including loading from and saving to a file.

#### Methods

- **`AddBookmarkAsync(string url, string title)`**
  - Adds a new bookmark and saves it to the file.
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.
  - Returns: `Task`.

- **`GetBookmarksAsync()`**
  - Retrieves all bookmarks by loading them from the file.
  - Returns: `Task<IEnumerable<Tuple<string, string>>>`.

- **`RemoveBookmarkAsync(string url)`**
  - Removes a bookmark by its URL and saves the updated list to the file.
  - Parameters:
    - `url`: The URL of the bookmark to remove.
  - Returns: `Task`.

- **`GetSelectedBookmarkAsync()`**
  - Retrieves the currently selected bookmark.
  - Returns: `Task<BookmarkItem>`.

- **`SelectBookmarkAsync(string url)`**
  - Selects a bookmark by its URL and saves the selection.
  - Parameters:
    - `url`: The URL of the bookmark to select.
  - Returns: `Task`.

- **`LoadBookmarksFromFileAsync()`**
  - Loads bookmarks from a file asynchronously.
  - Returns: `Task`.

- **`SaveBookmarksToFileAsync()`**
  - Saves the current list of bookmarks to a file asynchronously.
  - Returns: `Task`.

### Ailurus.CefGlueBrowserControl

Represents a browser control based on CefGlue, providing functionalities to navigate, reload, and manage the browser's state.

#### Constructor

- **`Ailurus.CefGlueBrowserControl.#ctor(AvaloniaCefBrowser browser)`**
  - Initializes a new instance of the `Ailurus.CefGlueBrowserControl` class with the specified browser instance.
  - Parameters:
    - `browser`: The `AvaloniaCefBrowser` instance to use within this control.
  - Exceptions:
    - Throws `ArgumentNullException` if the provided browser instance is null.

#### Methods

- **`InitializeControl()`**
  - Initializes the control by adding the browser instance to the logical children of the control.

- **`NavigateAsync(string url)`**
  - Navigates the browser to the specified URL asynchronously.
  - Parameters:
    - `url`: The URL to navigate to.
  - Returns: `Task`.

- **`Reload()`**
  - Reloads the current page in the browser.

- **`GoBack()`**
  - Navigates the browser back to the previous page, if possible.

- **`GoForward()`**
  - Navigates the browser forward to the next page, if possible.

- **`OpenDevTools()`**
  - Opens the developer tools for the browser.

#### Properties

- **`CurrentUrl`**
  - Gets or sets the current URL of the browser.

- **`Title`**
  - Gets the title of the current page loaded in the browser.

#### Utility Methods

- **`NormalizeUrl(string url)`**
  - Normalizes a URL by ensuring it is well-formed and prepending appropriate schemes if necessary.
  - Parameters:
    - `url`: The URL to normalize.
  - Returns: The normalized URL.

- **`IsLikelyHostOrIp(string url)`**
  - Determines if the given string is likely a host name or IP address.
  - Parameters:
    - `url`: The string to check.
  - Returns: `bool`.

- **`IsIpAddress(string url)`**
  - Checks if the provided string is a valid IP address.
  - Parameters:
    - `url`: The string to check.
  - Returns: `bool`.

### Ailurus.ConfigurationManager

Manages configuration settings including key bindings and the home URL.

#### Constructor

- **`Ailurus.ConfigurationManager.#ctor()`**
  - Initializes a new instance of `Ailurus.ConfigurationManager` and loads the configuration file, or uses default settings if the file is not found.

#### Methods

- **`GetKeyBindingsAsync()`**
  - Retrieves the key bindings from the configuration.
  - Returns: `Task<Dictionary<string, KeyGesture>>`.

- **`GetHomeUrlAsync()`**
  - Retrieves the home URL from the configuration.
  - Returns: `Task<string>`.

- **`SetKeyBindingAsync(string action, KeyGesture gesture)`**
  - Sets a key binding for a specific action and saves the configuration.
  - Parameters:
    - `action`: The action to associate with the key binding.
    - `gesture`: The key gesture representing the binding.
  - Returns: `Task`.

- **`SetHomeUrlAsync(string homeUrl)`**
  - Sets the home URL and saves the configuration.
  - Parameters:
    - `homeUrl`: The new home URL.
  - Returns: `Task`.

- **`RefreshSettingsAsync()`**
  - Refreshes the settings by reloading the configuration file.
  - Returns: `Task`.

- **`LoadConfigFile()`**
  - Loads the configuration data from the file.
  - Returns: The `ConfigurationData` object if the file is found, otherwise `null`.

- **`SaveConfigFileAsync()`**
  - Saves the current configuration data to the configuration file.
  - Returns: `Task`.

- **`GetDefaultConfig()`**
  - Provides the default configuration settings.
  - Returns: `ConfigurationData`.

- **`ParseKeyGesture(string gesture)`**
  - Parses a key gesture string into a `KeyGesture` object.
  - Parameters:
    - `gesture`: The string representation of the key gesture.
  - Returns: The parsed `KeyGesture` object.
  - Exceptions:
    - Throws `ArgumentException` if the key gesture string is invalid.

### Ailurus.HistoryItem

Represents a history item with a timestamp and URL.

#### Constructor

- **`Ailurus.HistoryItem.#ctor(DateTime timestamp, string url)`**
  - Parameters:
    - `timestamp`: The timestamp of when the history item was recorded.
    - `url`: The URL of the history item.

#### Methods

- **`ToString()`**
  - Returns a string representation of the history item.
  - Returns: A string in the format "Timestamp - URL".

### Ailurus.HistoryManager

Manages the browsing history, including adding, retrieving, and deleting history items.

#### Methods

- **`AddToHistoryAsync(string url)`**
  - Adds a new URL to the browsing history.
  - Parameters:
    - `url`: The URL to add to the history.
  - Returns: `Task`.

- **`GetHistoryAsync()`**
  - Retrieves the browsing history items in reverse chronological order.
  - Returns: `Task<IEnumerable<HistoryItem>>`.

- **`DeleteHistoryAsync()`**
  - Deletes all items from the browsing history.
  - Returns: `Task`.

- **`ClearHistoryAsync()`**
  - Clears the history list and deletes the history file if it exists.
  - Returns: `Task`.

- **`LoadHistoryFromFileAsync()`**
  - Loads the browsing history from the history file.
  - Returns: `Task`.

- **`SaveHistoryToFileAsync()`**
  - Saves the current browsing history to the history file.
  - Returns: `Task`.

### Ailurus.IBookmarkManager

Interface for managing bookmarks within the browser application.

#### Methods

- **`AddBookmarkAsync(string url, string title)`**
  - Adds a new bookmark with the specified URL and title.
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.
  - Returns: `Task`.

- **`GetBookmarksAsync()`**
  - Retrieves all bookmarks.
  - Returns: `Task<IEnumerable<Tuple<string, string>>>`.

- **`RemoveBookmarkAsync(string url)`**
  - Removes the bookmark with the specified URL.
  - Parameters:
    - `url`: The URL of the bookmark to remove.
  - Returns: `Task`.

- **`GetSelectedBookmarkAsync()`**
  - Gets the currently selected bookmark.
  - Returns: `Task<BookmarkItem>`.

- **`SelectBookmarkAsync(string url)`**
  - Selects a bookmark with the specified URL.
  - Parameters:
    - `url`: The URL of the bookmark to select.
  - Returns: `Task`.

### Ailurus.IConfigurationManager

Interface for managing application configuration settings, including key bindings and the home URL.

#### Methods

- **`GetKeyBindingsAsync()`**
  - Retrieves the key bindings for various actions.
  - Returns: `Task<Dictionary<string, KeyGesture>>`.

- **`SetKeyBindingAsync(string action, KeyGesture gesture)`**
  - Sets a key binding for a specific action.
  - Parameters:
    - `action`: The name of the action to bind.
    - `gesture`: The `KeyGesture` representing the key binding.
  - Returns: `Task`.

- **`GetHomeUrlAsync()`**
  - Retrieves the home URL set in the configuration.
  - Returns: `Task<string>`.

- **`SetHomeUrlAsync(string url)`**
  - Sets the home URL in the configuration.
  - Parameters:
    - `url`: The new home URL.
  - Returns: `Task`.

- **`RefreshSettingsAsync()`**
  - Refreshes the configuration settings by reloading them from the configuration file.
  - Returns: `Task`.

### Ailurus.IHistoryManager

Interface for managing the browsing history within the browser application.

#### Methods

- **`AddToHistoryAsync(string url)`**
  - Adds a new entry to the browsing history with the specified URL.
  - Parameters:
    - `url`: The URL to add to the history.
  - Returns: `Task`.

- **`GetHistoryAsync()`**
  - Retrieves the browsing history.
  - Returns: `Task<IEnumerable<HistoryItem>>`.

- **`DeleteHistoryAsync()`**
  - Deletes all entries from the browsing history.
  - Returns: `Task`.

### Ailurus.ISessionManager

Interface representing session management operations for the browser.

#### Methods

- **`SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs)`**
  - Saves the current session by persisting information about open tabs.
  - Parameters:
    - `tabs`: The collection of `BrowserTabViewModel` instances representing the open tabs.
  - Returns: `Task`.

- **`LoadSessionAsync(MainWindowViewModel window)`**
  - Loads a saved session by restoring tabs from previously persisted information.
  - Parameters:
    - `window`: The `MainWindowViewModel` where the tabs will be restored.
  - Returns: `Task<IEnumerable<BrowserTabViewModel>>`.

### Ailurus.MainWindow

The main window of the application, responsible for displaying the browser UI.

#### Constructor

- **`Ailurus.MainWindow.#ctor(MainWindowViewModel viewModel)`**
  - Initializes a new instance of the `Ailurus.MainWindow` class.
  - Parameters:
    - `viewModel`: The view model that will be used as the data context for this window.

#### Methods

- **`InitializeComponent(bool loadXaml)`**
  - Wires up the controls and optionally loads XAML markup and attaches dev tools (if Avalonia.Diagnostics package is referenced).
  - Parameters:
    - `loadXaml`: Should the XAML be loaded into the component.

### Ailurus.Program

The entry point of the application, responsible for initializing and starting the application.

#### Methods

- **`Main(string[] args)`**
  - The main entry point method for the application.
  - Parameters:
    - `args`: The command-line arguments.

- **`BuildAvaloniaApp(bool isAnonymousMode)`**
  - Builds the Avalonia application with the specified configuration.
  - Parameters:
    - `isAnonymousMode`: Indicates if the application should run in anonymous mode.
  - Returns: `Avalonia.AppBuilder`.

- **`InitializeCef(string[] args, bool isAnonymousMode)`**
  - Initializes the Chromium Embedded Framework (CEF) with the specified settings.
  - Parameters:
    - `args`: The command-line arguments.
    - `isAnonymousMode`: Indicates if the application should run in anonymous mode.

### Ailurus.SessionManager

Manages the session for the browser, including saving and loading tabs and cookies.

#### Methods

- **`SaveSessionAsync(IEnumerable<BrowserTabViewModel> tabs)`**
  - Saves the current session by persisting open tabs and cookies.
  - Parameters:
    - `tabs`: The collection of open tabs.
  - Returns: `Task`.

- **`LoadSessionAsync(MainWindowViewModel mainViewModel)`**
  - Loads a previously saved session, restoring open tabs and cookies.
  - Parameters:
    - `mainViewModel`: The view model for the main window.
  - Returns: `Task<IEnumerable<BrowserTabViewModel>>`.

- **`SaveCookiesAsync()`**
  - Saves browser cookies to a file.
  - Returns: `Task`.

- **`LoadCookiesAsync()`**
  - Loads browser cookies from a file.
  - Returns: `Task`.

#### Nested Types

- **`TabSessionData`**
  - Represents the data required to save a tab session.

- **`CookieData`**
  - Represents the data required to save a browser cookie.

- **`CookieVisitor`**
  - Visitor class for handling cookies during the saving process.

#### Methods in CookieVisitor

- **`CookieVisitor.#ctor(List<CookieData> cookies)`**
  - Initializes a new instance of the `CookieVisitor` class.

- **`Visit(CefCookie cookie, int count, int total, ref bool deleteCookie)`**
  - Visits each cookie and adds it to the list of cookies to be saved.
  - Parameters:
    - `cookie`: The current cookie being visited.
    - `count`: The current cookie's index.
    - `total`: The total number of cookies.
    - `deleteCookie`: Indicates whether the cookie should be deleted.
  - Returns: `bool`.

- **`WaitForCompletion()`**
  - Waits for the cookie visitation to complete.

### Ailurus.ViewModels

Contains various view models for managing the user interface and logic of the Ailurus browser.

#### BookmarkItemViewModel

Represents a single bookmark item in the UI.

##### Constructor

- **`BookmarkItemViewModel.#ctor(string url, string title, IBookmarkManager bookmarkManager, MainWindowViewModel mainWindowViewModel)`**
  - Initializes a new instance of the `BookmarkItemViewModel` class.
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.
    - `bookmarkManager`: The bookmark manager.
    - `mainWindowViewModel`: The main window view model.

##### Properties

- **`Url`**
  - Gets the URL of the bookmark.

- **`Title`**
  - Gets the title of the bookmark.

- **`SelectBookmarkCommand`**
  - Command to select the bookmark, navigating to its URL.

- **`RemoveBookmarkCommand`**
  - Command to remove the bookmark.

##### Methods

- **`SelectBookmark()`**
  - Selects the bookmark by setting the URL in the main window and executing the Go command.

- **`RemoveBookmarkAsync()`**
  - Removes the bookmark asynchronously and reloads the bookmarks list in the main window.

#### BrowserTabViewModel

Represents a single browser tab.

##### Properties

- **`Header`**
  - Gets or sets the tab header, usually the page title.

- **`Url`**
  - Gets or sets the current URL of the browser tab.

- **`IsLoading`**
  - Gets or sets a value indicating whether the browser is currently loading.

- **`IsSelected`**
  - Gets or sets a value indicating whether the tab is currently selected.

- **`Browser`**
  - Gets the `AvaloniaCefBrowser` instance for this tab.

- **`BrowserControl`**
  - Gets the `CefGlueBrowserControl` instance for managing browser operations.

##### Constructor

- **`BrowserTabViewModel.#ctor(MainWindowViewModel mainWindowViewModel)`**
  - Initializes a new instance of the `BrowserTabViewModel` class.
  - Parameters:
    - `mainWindowViewModel`: The view model for the main window.

##### Methods

- **`OnBrowserAddressChanged(object sender, string address)`**
  - Handles the `AddressChanged` event to update the `Url` property.

- **`OnBrowserTitleChanged(object sender, string title)`**
  - Handles the `TitleChanged` event to update the `Header` property.

- **`NavigateAsync(string url)`**
  - Navigates the browser to the specified URL asynchronously.
  - Parameters:
    - `url`: The URL to navigate to.
  - Returns: `Task`.

- **`SetIsSelected(bool isSelected)`**
  - Sets the `IsSelected` property for this tab.
  - Parameters:
    - `isSelected`: Whether the tab is selected.

- **`CloseTabCommand`**
  - Command to close the tab.

- **`SelectTabCommand`**
  - Command to select the tab.

#### HistoryItemViewModel

Represents a single history item.

##### Constructor

- **`HistoryItemViewModel.#ctor(DateTime timestamp, string url)`**
  - Initializes a new instance of the `HistoryItemViewModel` class.
  - Parameters:
    - `timestamp`: The timestamp of the history item.
    - `url`: The URL of the history item.

##### Properties

- **`Timestamp`**
  - Gets the timestamp of the history item.

- **`Url`**
  - Gets the URL of the history item.

#### HistoryWindowViewModel

ViewModel for managing the history window.

##### Constructor

- **`HistoryWindowViewModel.#ctor(IHistoryManager historyManager)`**
  - Initializes a new instance of the `HistoryWindowViewModel` class.
  - Parameters:
    - `historyManager`: The history manager.

##### Properties

- **`HistoryItems`**
  - Gets the collection of history items.

- **`ClearHistoryCommand`**
  - Command to clear the history.

##### Methods

- **`LoadHistoryAsync()`**
  - Loads the browsing history asynchronously and populates the `HistoryItems` collection.

- **`ClearHistoryAsync()`**
  - Clears the browsing history asynchronously.

#### MainWindowViewModel

ViewModel for the main window of the Ailurus web browser. Manages tabs, bookmarks, and key bindings.

##### Properties

- **`Tabs`**
  - Gets the collection of browser tabs.

- **`Bookmarks`**
  - Gets the collection of bookmarks.

- **`DynamicKeyBindings`**
  - Gets the collection of dynamically loaded key bindings.

- **`SelectedTab`**
  - Gets or sets the currently selected tab.

- **`BrowserContent`**
  - Gets the browser content of the selected tab.

- **`EditableUrl`**
  - Gets or sets the editable URL in the address bar.

##### Constructor

- **`MainWindowViewModel.#ctor(ISessionManager sessionManager, IHistoryManager historyManager, IBookmarkManager bookmarkManager, IConfigurationManager configManager)`**
  - Initializes a new instance of the `MainWindowViewModel` class.
  - Parameters:
    - `sessionManager`: The session manager.
    - `historyManager`: The history manager.
    - `bookmarkManager`: The bookmark manager.
    - `configManager`: The configuration manager.

##### Methods

- **`InitializeCommands()`**
  - Initializes the commands for the main window.

- **`UpdateCommands()`**
  - Raises property changed events for the commands.

- **`RefreshSettingsAsync()`**
  - Refreshes the settings by reloading them from the configuration manager.

- **`InitializeKeyBindings()`**
  - Initializes key bindings from the configuration and sets up commands.

- **`AddKeyBinding(KeyGesture gesture, ICommand command)`**
  - Adds a key binding to the dynamic key bindings collection.
  - Parameters:
    - `gesture`: The key gesture.
    - `command`: The command to execute.

- **`AddNewTab()`**
  - Adds a new browser tab.

- **`GoToUrlAsync()`**
  - Navigates to the URL entered in the address bar.
  - Returns: `Task`.

- **`UpdateEditableUrl()`**
  - Updates the editable URL with the selected tab's URL.

- **`OnSelectedTabUrlChanged(object sender, PropertyChangedEventArgs e)`**
  - Handles URL change events for the selected tab.

- **`OnSelectedTabTitleChanged(object sender, PropertyChangedEventArgs e)`**
  - Handles title change events for the selected tab.

- **`NavigateUp()`**
  - Navigates to the previous tab in the list.

- **`NavigateDown()`**
  - Navigates to the next tab in the list.

- **`LoadBookmarksAsync()`**
  - Loads the bookmarks from the bookmark manager.

- **`AddBookmark()`**
  - Adds the current URL as a bookmark.

- **`CloseTab(BrowserTabViewModel tab)`**
  - Closes the specified tab.
  - Parameters:
    - `tab`: The tab to close.

- **`SaveSessionAsync()`**
  - Saves the current session.

- **`LoadSessionAsync()`**
  - Loads the session from the session manager.

- **`OpenHistoryWindow()`**
  - Opens the history window.


### Ailurus.Converters.SelectedTabConverter

Converts a boolean value indicating the selection status of a tab to a corresponding background color. This is used to visually differentiate the selected tab from the others in the UI.

#### Methods

- **`Convert(object? value, Type targetType, object? parameter, CultureInfo culture)`**
  - Converts a boolean value indicating whether a tab is selected to a color.
  - Parameters:
    - `value`: A boolean value indicating whether the tab is selected.
    - `targetType`: The type of the binding target property.
    - `parameter`: Additional parameter for the converter (not used).
    - `culture`: The culture to use in the converter.
  - Returns: A `SolidColorBrush` corresponding to the selected or unselected state of the tab.

- **`ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)`**
  - Converts back the color to a boolean, but this operation is not supported.
  - Parameters:
    - `value`: The value produced by the binding target.
    - `targetType`: The type to convert to.
    - `parameter`: Additional parameter for the converter (not used).
    - `culture`: The culture to use in the converter.
  - Returns: Throws `NotSupportedException` because conversion back is not supported.

### Ailurus.ViewModels.BookmarkItemViewModel

Represents a single bookmark item in the UI.

#### Constructor

- **`BookmarkItemViewModel.#ctor(string url, string title, IBookmarkManager bookmarkManager, MainWindowViewModel mainWindowViewModel)`**
  - Initializes a new instance of the `BookmarkItemViewModel` class.
  - Parameters:
    - `url`: The URL of the bookmark.
    - `title`: The title of the bookmark.
    - `bookmarkManager`: The bookmark manager.
    - `mainWindowViewModel`: The main window view model.

#### Properties

- **`Url`**
  - Gets the URL of the bookmark.

- **`Title`**
  - Gets the title of the bookmark.

- **`SelectBookmarkCommand`**
  - Command to select the bookmark, navigating to its URL.

- **`RemoveBookmarkCommand`**
  - Command to remove the bookmark.

#### Methods

- **`SelectBookmark()`**
  - Selects the bookmark by setting the URL in the main window and executing the Go command.

- **`RemoveBookmarkAsync()`**
  - Removes the bookmark asynchronously and reloads the bookmarks list in the main window.

### Ailurus.ViewModels.BrowserTabViewModel

Represents a single browser tab in the UI.

#### Constructor

- **`BrowserTabViewModel.#ctor(MainWindowViewModel mainWindowViewModel)`**
  - Initializes a new instance of the `BrowserTabViewModel` class.
  - Parameters:
    - `mainWindowViewModel`: The main window view model.

#### Properties

- **`Header`**
  - Gets or sets the tab header, usually the page title.

- **`Url`**
  - Gets or sets the current URL of the browser tab.

- **`IsLoading`**
  - Gets or sets a value indicating whether the browser is currently loading.

- **`IsSelected`**
  - Gets or sets a value indicating whether the tab is currently selected.

- **`Browser`**
  - Gets the `AvaloniaCefBrowser` instance for this tab.

- **`BrowserControl`**
  - Gets the `CefGlueBrowserControl` instance for managing browser operations.

#### Methods

- **`OnBrowserAddressChanged(object sender, string address)`**
  - Handles the `AddressChanged` event to update the `Url` property.

- **`OnBrowserTitleChanged(object sender, string newTitle)`**
  - Handles the `TitleChanged` event to update the `Header` property.

- **`NavigateAsync(string url)`**
  - Navigates the browser to the specified URL asynchronously.
  - Parameters:
    - `url`: The URL to navigate to.
  - Returns: `Task`.

- **`SetIsSelected(bool isSelected)`**
  - Sets the `IsSelected` property for this tab.
  - Parameters:
    - `isSelected`: Whether the tab is selected.

- **`CloseTabCommand`**
  - Command to close the tab.

- **`SelectTabCommand`**
  - Command to select the tab.

### Ailurus.ViewModels.HistoryItemViewModel

Represents a single history item in the UI.

#### Constructor

- **`HistoryItemViewModel.#ctor(DateTime timestamp, string url)`**
  - Initializes a new instance of the `HistoryItemViewModel` class.
  - Parameters:
    - `timestamp`: The timestamp of the history item.
    - `url`: The URL of the history item.

#### Properties

- **`Timestamp`**
  - Gets the timestamp of the history item.

- **`Url`**
  - Gets the URL of the history item.

### Ailurus.ViewModels.HistoryWindowViewModel

ViewModel for managing the history window.

#### Constructor

- **`HistoryWindowViewModel.#ctor(IHistoryManager historyManager)`**
  - Initializes a new instance of the `HistoryWindowViewModel` class.
  - Parameters:
    - `historyManager`: The history manager.

#### Properties

- **`HistoryItems`**
  - Gets the collection of history items.

- **`ClearHistoryCommand`**
  - Command to clear the history.

#### Methods

- **`LoadHistoryAsync()`**
  - Loads the browsing history asynchronously and populates the `HistoryItems` collection.

- **`ClearHistoryAsync()`**
  - Clears the browsing history asynchronously.

### Ailurus.ViewModels.MainWindowViewModel

ViewModel for the main window of the Ailurus web browser. Manages tabs, bookmarks, and key bindings.

#### Constructor

- **`MainWindowViewModel.#ctor(ISessionManager sessionManager, IHistoryManager historyManager, IBookmarkManager bookmarkManager, IConfigurationManager configManager)`**
  - Initializes a new instance of the `MainWindowViewModel` class.
  - Parameters:
    - `sessionManager`: The session manager.
    - `historyManager`: The history manager.
    - `bookmarkManager`: The bookmark manager.
    - `configManager`: The configuration manager.

#### Properties

- **`Tabs`**
  - Gets the collection of browser tabs.

- **`Bookmarks`**
  - Gets the collection of bookmarks.

- **`DynamicKeyBindings`**
  - Gets the collection of dynamically loaded key bindings.

- **`SelectedTab`**
  - Gets or sets the currently selected tab.

- **`BrowserContent`**
  - Gets the browser content of the selected tab.

- **`EditableUrl`**
  - Gets or sets the editable URL in the address bar.

- **`AddNewTabCommand`**
  - Command to add a new browser tab.

- **`GoCommand`**
  - Command to navigate to the URL entered in the address bar.

- **`BackCommand`**
  - Command to navigate back in the browser's history.

- **`ForwardCommand`**
  - Command to navigate forward in the browser's history.

- **`ReloadCommand`**
  - Command to reload the current page.

- **`OpenDevToolsCommand`**
  - Command to open the browser's developer tools.

- **`OpenHistoryCommand`**
  - Command to open the history window.

- **`AddBookmarkCommand`**
  - Command to add the current URL as a bookmark.

- **`RefreshSettingsCommand`**
  - Command to refresh settings from the configuration manager.

- **`NavigateUpCommand`**
  - Command to navigate to the previous tab in the list.

- **`NavigateDownCommand`**
  - Command to navigate to the next tab in the list.

- **`BackGesture`**
  - Key gesture for navigating back.

- **`ForwardGesture`**
  - Key gesture for navigating forward.

- **`ReloadGesture`**
  - Key gesture for reloading the page.

- **`GoGesture`**
  - Key gesture for navigating to the URL in the address bar.

- **`AddBookmarkGesture`**
  - Key gesture for adding a bookmark.

- **`OpenHistoryGesture`**
  - Key gesture for opening the history window.

- **`NavigateUpGesture`**
  - Key gesture for navigating to the previous tab.

- **`NavigateDownGesture`**
  - Key gesture for navigating to the next tab.

#### Methods

- **`InitializeCommands()`**
  - Initializes the commands for the main window.

- **`UpdateCommands()`**
  - Raises property changed events for the commands.

- **`RefreshSettingsAsync()`**
  - Refreshes the settings by reloading them from the configuration manager.

- **`InitializeKeyBindings()`**
  - Initializes key bindings from the configuration and sets up commands.

- **`AddKeyBinding(KeyGesture gesture, ICommand command)`**
  - Adds a key binding to the dynamic key bindings collection.
  - Parameters:
    - `gesture`: The key gesture.
    - `command`: The command to execute.

- **`AddNewTab()`**
  - Adds a new browser tab.

- **`GoToUrlAsync()`**
  - Navigates to the URL entered in the address bar.

- **`UpdateEditableUrl()`**
  - Updates the editable URL with the selected tab's URL.

- **`OnSelectedTabUrlChanged(object sender, PropertyChangedEventArgs e)`**
  - Handles URL change events for the selected tab.

- **`OnSelectedTabTitleChanged(object sender, PropertyChangedEventArgs e)`**
  - Handles title change events for the selected tab.

- **`NavigateUp()`**
  - Navigates to the previous tab in the list.

- **`NavigateDown()`**
  - Navigates to the next tab in the list.

- **`LoadBookmarksAsync()`**
  - Loads the bookmarks from the bookmark manager.

- **`AddBookmark()`**
  - Adds the current URL as a bookmark.

- **`CloseTab(BrowserTabViewModel tab)`**
  - Closes the specified tab.
  - Parameters:
    - `tab`: The tab to close.

- **`SaveSessionAsync()`**
  - Saves the current session.

- **`LoadSessionAsync()`**
  - Loads the session from the session manager.

- **`OpenHistoryWindow()`**
  - Opens the history window.