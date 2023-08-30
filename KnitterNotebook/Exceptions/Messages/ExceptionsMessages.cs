namespace KnitterNotebook.Exceptions.Messages
{
    public class ExceptionsMessages
    {
        public static string UserWithIdNotFound(int id) => $"User with id {id} was not found";

        public static string UserWithEmailNotFound(string email) => $"User with e-mail {email} was not found";

        public static string EntityWithIdNotFound(int id) => $"Entity with id {id} was not found";

        public static string ThemeWithNameNotFound(string themeName) => $"Theme with name {themeName} was not found";
        
        public static string NullFileWhenSave => $"File was not saved because path was null";
    }
}