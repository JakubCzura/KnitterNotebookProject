using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services
{
    public class ImageService : CrudService<Image>, IImageService
    {
        private readonly DatabaseContext _databaseContext;
        public ImageService(DatabaseContext databaseContext) : base(databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateAsync(CreateImageDto createImageDto)
        {
            Image image = new(createImageDto.Path);
            await _databaseContext.AddAsync(image);
            await _databaseContext.SaveChangesAsync();
        }
    }
}
