using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;

namespace KnitterNotebook.Validators
{
    public class MovieUrlValidator : IModelsValidator<MovieUrl>
    {
        public bool Validate(MovieUrl movieUrl)
        {
            Guard.IsNotNull(movieUrl.User);
            Guard.IsNotNullOrWhiteSpace(movieUrl.Title);
            Guard.IsNotNull(movieUrl.Link);
            return true;
        }
    }
}