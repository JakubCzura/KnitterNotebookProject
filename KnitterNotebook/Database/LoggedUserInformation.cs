using KnitterNotebook.Models;

namespace KnitterNotebook.Database
{
    public class LoggedUserInformation
    {
        public static User LoggedUser { get; set; } = null!;
    }
}