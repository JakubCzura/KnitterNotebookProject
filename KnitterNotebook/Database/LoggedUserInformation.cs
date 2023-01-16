using KnitterNotebook.Models;

namespace KnitterNotebook.Database
{
    public class LoggedUserInformation
    {
        /// <summary>
        /// Stores information about user who is actually logged in to application
        /// </summary>
        public static User LoggedUser { get; set; } = null!;
    }
}