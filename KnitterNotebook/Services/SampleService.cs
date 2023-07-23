using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class SampleService : CrudService<Sample>, ISampleService
    {
        private readonly DatabaseContext _databaseContext;

        public SampleService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<bool> CreateAsync(CreateSampleDto createSampleDto)
        {
            Image? image = !string.IsNullOrWhiteSpace(createSampleDto.DestinationImagePath)
                            ? new(createSampleDto.DestinationImagePath)
                            : null;

            Sample sample = new()
            {
                YarnName = createSampleDto.YarnName,
                LoopsQuantity = createSampleDto.LoopsQuantity,
                RowsQuantity = createSampleDto.RowsQuantity,
                NeedleSize = createSampleDto.NeedleSize,
                NeedleSizeUnit = createSampleDto.NeedleSizeUnit,
                Description = createSampleDto.Description,
                UserId = createSampleDto.UserId,
                Image = image,
            };

            if (!string.IsNullOrWhiteSpace(createSampleDto.SourceImagePath) && !string.IsNullOrWhiteSpace(createSampleDto.DestinationImagePath))
                FileHelper.CopyWithDirectoryCreation(createSampleDto.SourceImagePath, createSampleDto.DestinationImagePath);

            await _databaseContext.Samples.AddAsync(sample);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}