using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;

namespace KnitterNotebook.Validators
{
    public class ProjectValidator : IValidator<Project>
    {
        public bool Validate(Project project)
        {
            Guard.IsNotNull(project.User);
            Guard.IsNotNullOrWhiteSpace(project.Name);
            Guard.HasSizeLessThanOrEqualTo(project.Name, 100);
            return true;
        }
    }
}