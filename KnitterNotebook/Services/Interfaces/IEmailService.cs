using KnitterNotebook.Models.Dtos;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(SendEmailDto sendEmailDto);
    }
}