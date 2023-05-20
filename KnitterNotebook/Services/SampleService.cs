using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class SampleService : CrudService<Sample>, ISampleService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;

        public SampleService(DatabaseContext databaseContext, IImageService imageService, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _imageService = imageService;
            _userService = userService;
        }

        public async Task<bool> CreateAsync(CreateSampleDto createSampleDto)
        {
            Image? image = null;
            if (!string.IsNullOrWhiteSpace(createSampleDto.ImagePath))
            {
                image = new(createSampleDto.ImagePath);
                await _imageService.CreateAsync(image);
            }
            Sample sample = new()
            {
                YarnName = createSampleDto.YarnName,
                LoopsQuantity = createSampleDto.LoopsQuantity,
                RowsQuantity = createSampleDto.RowsQuantity,
                NeedleSize = createSampleDto.NeedleSize,
                NeedleSizeUnit = createSampleDto.NeedleSizeUnit,
                Description = createSampleDto.Description,
                User = await _userService.GetAsync(LoggedUserInformation.Id),
                ImageId = image?.Id
            };
            await _databaseContext.Samples.AddAsync(sample);
            await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}