namespace KnitterNotebook.Exceptions
{
    public class ExceptionsMessages
    {
        public static string UserWithIdNotFound(int id) => $"User with {id} was not found";
        public static string UserWithNicknameOrEmailNotFound(string nicknameOrEmail) => $"User with {nicknameOrEmail} was not found";
    }
}