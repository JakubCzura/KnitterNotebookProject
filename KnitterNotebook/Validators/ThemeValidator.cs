using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;
using System;
using System.Windows;

namespace KnitterNotebook.Validators
{
    public class ThemeValidator : IValidator<Theme>
    {
        public bool Validate(Theme theme)
        {
            try
            {
                Guard.IsNotNullOrWhiteSpace(theme.Name);
                Guard.HasSizeLessThanOrEqualTo(theme.Name, 50);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
            return true;
        }
    }
}