using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KnitterNotebook.Helpers.Extensions;
using KnitterNotebook.Helpers;

namespace KnitterNotebook.Services
{
    public class SampleService : CrudService<Sample>, ISampleService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IUserService _userService;

        public SampleService(DatabaseContext databaseContext, IUserService userService) : base(databaseContext)
        {
            _databaseContext = databaseContext;
            _userService = userService;
        }

        public async Task<int> CreateAsync(CreateSampleDto createSampleDto)
        {
            string? nickname = await _userService.GetNicknameAsync(createSampleDto.UserId)
                                ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(createSampleDto.UserId));

            string? destinationImagePath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(createSampleDto.SourceImagePath));

            SampleImage? image = !string.IsNullOrWhiteSpace(destinationImagePath) ? new(destinationImagePath) : null;

            Sample sample = new()
            {
                YarnName = createSampleDto.YarnName,
                LoopsQuantity = createSampleDto.LoopsQuantity,
                RowsQuantity = createSampleDto.RowsQuantity,
                NeedleSize = createSampleDto.NeedleSize,
                NeedleSizeUnit = createSampleDto.NeedleSizeUnit,
                Description = createSampleDto.Description,
                UserId = createSampleDto.UserId,
                Image = image
            };

            if (!string.IsNullOrWhiteSpace(createSampleDto.SourceImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
                FileHelper.CopyWithDirectoryCreation(createSampleDto.SourceImagePath, destinationImagePath);

            await _databaseContext.Samples.AddAsync(sample);
            return await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<SampleDto>> GetUserSamplesAsync(int userId)
            => await _databaseContext.Samples.Include(x => x.Image)
                                             .Where(x => x.UserId == userId)
                                             .Select(x => new SampleDto(x))
                                             .ToListAsync();
    }
}