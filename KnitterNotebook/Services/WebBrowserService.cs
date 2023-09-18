using KnitterNotebook.Services.Interfaces;
using System;
using System.Diagnostics;

namespace KnitterNotebook.Services;

public class WebBrowserService : IWebBrowserService
{
    /// <summary>
    /// Opens the URL in the default web browser
    /// </summary>
    /// <param name="link">URL to open</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Open(Uri link)
    {
        if (link is null)
        {
            throw new ArgumentNullException(nameof(link));
        }
        Process.Start("cmd", $"/C start {link}");
    }
}