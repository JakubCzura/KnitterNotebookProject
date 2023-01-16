using KnitterNotebook.Models;
using System;
using System.Windows;

namespace KnitterNotebook.Validators
{
    public class ProjectValidator : IValidator<Project>
    {
        public bool Validate(Project project)
        {
            try
            {
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