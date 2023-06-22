using System.Diagnostics;

namespace KnitterNotebook.Helpers
{
    public class WebBrowserHelper
    {
        /// <summary>
        /// Opens the URL in the default web browser
        /// </summary>
        /// <param name="link">URL to open</param>
        public static void OpenUrlInWebBrowser(string link)
        {
            Process.Start("cmd", $"/C start {link}");
        }
    }
}