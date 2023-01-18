using CommunityToolkit.Diagnostics;
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
                Guard.IsNotNullOrWhiteSpace(project.Name);
                Guard.HasSizeLessThanOrEqualTo(project.Name, 100);
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