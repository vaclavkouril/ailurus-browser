# Ailurus Web Browser - User Documentation

## Overview

Ailurus is a modern web browser designed with a user-friendly interface that allows you to navigate the web, manage bookmarks, and organize your browsing sessions with tabs. This documentation provides an overview of the browser's key features. Additionally, Ailurus offers an **Anonymous Mode** for private browsing, which can be activated with a command-line argument.

## User Interface

### Header Section

At the top of the Ailurus browser window, you'll find the **Header Section**, which includes several essential controls:

- **Back Button**: Navigate to the previous page in your browsing history.
- **Forward Button**: Navigate forward to the next page in your browsing history.
- **URL Box**: A text box where you can enter a web address (URL) to visit a site. Press `Enter` or click the "Go" button to navigate.
- **Go Button**: Click to navigate to the URL entered in the URL Box.
- **Refresh Button**: Reloads the current webpage.
- **Bookmark Button**: Add the current webpage to your bookmarks.
- **Settings Button**: Provides additional options such as opening the history window.

### Bookmarks Row

Just below the header, the **Bookmarks Row** displays a list of your saved bookmarks. Each bookmark is represented by a button with the following features:

- **Title**: The name of the bookmarked page, displayed on the button.
- **Select Bookmark Button**: Clicking this will navigate to the bookmarked page.
- **Remove Bookmark Button**: An "x" button to delete the bookmark from your list.

You can scroll horizontally through your bookmarks if you have many saved.

### Tabs Section

The **Tabs Section** on the left side of the browser displays all open tabs and allows for easy tab management:

- **Tab List**: A vertical list of open tabs. Each tab displays the page title and has a close button ("x") to remove the tab.
- **New Tab Button**: A "+" button located at the bottom of the tab list. Clicking this will open a new tab.

### Browser Content

The main part of the window, on the right, is where the **Browser Content** is displayed. This area shows the web page content for the selected tab.

### Key Commands

Ailurus supports a range of key commands to streamline your browsing experience:

- **Navigate Up**: `Ctrl+Up`
  - Move to the previous tab in the list.
  
- **Navigate Down**: `Ctrl+Down`
  - Move to the next tab in the list.
  
- **Add Bookmark**: `Ctrl+B`
  - Add the current page to your bookmarks.
  
- **Go**: `Ctrl+G`
  - Navigate to the URL entered in the URL box.
  
- **Back**: `Ctrl+Left`
  - Navigate to the previous page in the browser history.
  
- **Forward**: `Ctrl+Right`
  - Navigate to the next page in the browser history.
  
- **Reload**: `Ctrl+R`
  - Reload the current page.

## Tabs Functionality

Tabs allow you to keep multiple pages open simultaneously and switch between them easily:

- **Opening a New Tab**: Click the "+" button in the Tabs Column to open a new tab. The new tab will load your home page by default.
- **Switching Tabs**: You can switch between tabs by clicking on the tab in the tab list or using the `Ctrl+UpArrow` and `Ctrl+DownArrow` key commands.
- **Closing Tabs**: To close a tab, click the "x" button on the tab. If you close the currently active tab, the browser will automatically select the next tab.

## Bookmark Management

Bookmarks in Ailurus help you quickly access your favorite or frequently visited pages:

- **Adding a Bookmark**: Click the "Bookmark" button in the header or use the `Ctrl+B` key command to save the current page as a bookmark. The bookmark will appear in the Bookmarks Row.
- **Accessing Bookmarks**: Click any bookmark in the Bookmarks Row to navigate to that page.
- **Removing Bookmarks**: Click the "x" button on a bookmark to remove it from your list.

## URL Box

The **URL Box** is located in the Header Section and serves as the primary input field for navigating to different web pages:

- **Entering a URL**: Type or paste a web address into the URL Box and press `Enter` or click the "Go" button to load the page.
- **Editing the URL**: The URL Box automatically updates with the current tab's address, allowing you to edit and navigate easily.

## Anonymous Mode

Ailurus includes an **Anonymous Mode** that allows you to browse the web without saving any history, cookies, or bookmarks. This mode is ideal for private browsing sessions.

### How to Launch Anonymous Mode

To start Ailurus in Anonymous Mode, use the `--anonymous` argument when launching the application from the command line:

```bash
Ailurus --anonymous
