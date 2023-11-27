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
    /// <exception cref="ArgumentNullException">When <paramref name="link"/> is null</exception>
    public void Open(Uri link)
    {
        ArgumentNullException.ThrowIfNull(link);
        Process.Start(new ProcessStartInfo(link.AbsoluteUri) { UseShellExecute = true });
    }
}