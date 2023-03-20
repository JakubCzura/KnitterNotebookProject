using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;

namespace KnitterNotebook.Validators
{
    public class ThemeValidator : IValidator<Theme>
    {
        public bool Validate(Theme theme)
        {
            Guard.IsNotNullOrWhiteSpace(theme.Name);
            Guard.HasSizeLessThanOrEqualTo(theme.Name, 50);
            return true;
        }
    }
}