using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IPatternPdfService : ICrudService<PatternPdf>
    {
        public Task Create(CreatePatternPdfDto createPatternPdfDto);
    }
}
