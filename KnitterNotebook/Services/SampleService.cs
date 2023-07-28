using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class SampleService : CrudService<Sample>, ISampleService
    {
        private readonly DatabaseContext _databaseContext;

        public SampleService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        private readonly IUserService _userService;

        public async Task<bool> CreateAsync(CreateSampleDto createSampleDto)
        {
            string? nickname = await _userService.GetNicknameAsync(LoggedUserInformation.Id);
            string? destinationImagePath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(createSampleDto.SourceImagePath));

            Image? image = !string.IsNullOrWhiteSpace(destinationImagePath)
                            ? new(destinationImagePath)
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

            if (!string.IsNullOrWhiteSpace(createSampleDto.SourceImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
                FileHelper.CopyWithDirectoryCreation(createSampleDto.SourceImagePath, destinationImagePath);

            await _databaseContext.Samples.AddAsync(sample);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}