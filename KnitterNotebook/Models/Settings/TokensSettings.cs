namespace KnitterNotebook.Models.Settings;

public class TokensSettings
{
    public const string SectionKey = "Tokens";
    public int ResetPasswordTokenExpirationDays { get; set; }
}