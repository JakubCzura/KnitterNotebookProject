namespace KnitterNotebook.Models.Settings;

public class TokensSettings
{
    public static string SectionKey => "Tokens";
    public int ResetPasswordTokenExpirationDays { get; set; }
}