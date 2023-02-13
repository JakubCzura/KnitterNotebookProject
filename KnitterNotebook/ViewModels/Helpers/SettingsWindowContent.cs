using KnitterNotebook.Views.UserControls;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KnitterNotebook.ViewModels.Helpers
{
    public class SettingsWindowContent
    {
        /// <summary>
        /// Chooses content of SettingsWindow - user's data and other stuff that user wants to see
        /// </summary>
        /// <param name="userControlName">Name of user control, for example UserSettingsUserControl</param>
        /// <returns>New instance of chosen user control</returns>    
        public static UserControl ChooseSettingsWindowContent(string userControlName)
        {
            return userControlName switch
            {
                nameof(UserSettingsUserControl) => new UserSettingsUserControl(),
                nameof(ThemeSettingsUserControl) => new ThemeSettingsUserControl(),
                _ => new UserSettingsUserControl()
            };
        }
    }
}
