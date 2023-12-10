using MailKit.Security;

namespace KnitterNotebook.Models.Settings;

public class SmtpSettings
{
    public static string SectionKey => "SmtpSettings";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public SecureSocketOptions SecureSocketOptions { get; set; }
}