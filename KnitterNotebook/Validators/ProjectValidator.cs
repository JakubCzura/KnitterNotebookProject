using CommunityToolkit.Diagnostics;
using KnitterNotebook.Migrations;
using KnitterNotebook.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace KnitterNotebook.Validators
{
    public class ProjectValidator : IValidator<Project>
    {
        public bool Validate(Project project)
        {
            try
            {
                Guard.IsNotNull(project.UserId);
           

        public DateTime StartDate { get; set; } = new();

        public DateTime? EndDate { get; set; } = null!;

        public int UserId { get; set; }

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