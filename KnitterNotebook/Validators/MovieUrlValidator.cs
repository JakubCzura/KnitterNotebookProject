using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;
using System;
using System.Windows;

namespace KnitterNotebook.Validators
{
    public class MovieUrlValidator : IValidator<MovieUrl>
    {
        public bool Validate(MovieUrl movieUrl)
        {
            try
            {
                Guard.IsNotNull(movieUrl.User);
                Guard.IsNotNullOrWhiteSpace(movieUrl.Title);
                Guard.IsNotNull(movieUrl.Link);
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