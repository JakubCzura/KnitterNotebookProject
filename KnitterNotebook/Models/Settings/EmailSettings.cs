namespace KnitterNotebook.Models.Settings;

public class EmailSettings
{
    public const string SectionKey = "EmailSending";
    public string SenderName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}