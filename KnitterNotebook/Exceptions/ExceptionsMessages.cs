namespace KnitterNotebook.Exceptions
{
    public class ExceptionsMessages
    {
        public static string UserWithIdNotFound(int id) => $"Użytkownik z id {id} nie został znaleziony";

        public static string UserWithNicknameOrEmailNotFound(string nicknameOrEmail) => $"Użytkownik o nicku lub e-mail {nicknameOrEmail} nie został znaleziony";

        public static string EntityWithIdNotFound(int id) => $"Rekord z id {id} nie został znaleziony";

        public static string ThemeWithNameNotFound(string themeName) => $"Motyw o nazwie {themeName} nie został znaleziony";
    }
}