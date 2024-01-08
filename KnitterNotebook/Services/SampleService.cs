using KnitterNotebook.ApplicationInformation;
using KnitterNotebook.Database;
using KnitterNotebook.Exceptions;
using KnitterNotebook.Exceptions.Messages;
using KnitterNotebook.Helpers;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models.Entities;
using KnitterNotebook.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KnitterNotebook.Services;

public class SampleService(DatabaseContext databaseContext,
                           IUserService userService) : CrudService<Sample>(databaseContext), ISampleService
{
    private readonly DatabaseContext _databaseContext = databaseContext;
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Creates sample and saves it do database
    /// </summary>
    /// <param name="createSampleDto">Data to create sample</param>
    /// <returns>Quantity of entities saved to database</returns>
    /// <exception cref="EntityNotFoundException">When user doesn't exist in database</exception>
    public async Task<int> CreateAsync(CreateSampleDto createSampleDto)
    {
        //User can create sample without photo, but it would be impossible situation if nickname was null so exception is thrown when nickname is null
        string? nickname = await _userService.GetNicknameAsync(createSampleDto.UserId)
                            ?? throw new EntityNotFoundException(ExceptionsMessages.UserWithIdNotFound(createSampleDto.UserId));

        string? destinationImagePath = Paths.PathToSaveUserFile(nickname, Path.GetFileName(createSampleDto.SourceImagePath));
        SampleImage? image = !string.IsNullOrWhiteSpace(destinationImagePath) ? new(destinationImagePath) : null;

        Sample sample = new(createSampleDto, image);

        if (!string.IsNullOrWhiteSpace(createSampleDto.SourceImagePath) && File.Exists(createSampleDto.SourceImagePath) && !string.IsNullOrWhiteSpace(destinationImagePath))
        {
            FileHelper.CopyWithDirectoryCreation(createSampleDto.SourceImagePath, destinationImagePath);
        }

        await _databaseContext.Samples.AddAsync(sample);
        return await _databaseContext.SaveChangesAsync();
    }

    /// <summary>
    /// Returns all samples of user with given <paramref name="userId"/> from database
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>User's all samples</returns>
    public async Task<List<SampleDto>> GetUserSamplesAsync(int userId)
        => await _databaseContext.Samples.AsNoTracking()
                                         .Include(x => x.Image)
                                         .Where(x => x.UserId == userId)
                                         .Select(x => new SampleDto(x))
                                         .ToListAsync();
}