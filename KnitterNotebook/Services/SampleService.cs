using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Repositories;
using KnitterNotebook.Repositories.Interfaces;
using KnitterNotebook.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task CreateAsync(CreateSampleDto createSampleDto)
        {
            throw new NotImplementedException("SampleService - CreateAsync");
            
            //Sample sample = new()
            //{
            //    YarnName = createSampleDto.YarnName,
            //    LoopsQuantity = createSampleDto.LoopsQuantity,
            //    RowsQuantity = createSampleDto.RowsQuantity,
            //    NeedleSize = createSampleDto.NeedleSize,
            //    NeedleSizeUnit = createSampleDto.NeedleSizeUnit,
            //    Description = createSampleDto.Description,
            //    User = createSampleDto.User,
            //    Image = createSampleDto.Image
            //};
            //await _databaseContext.Samples.AddAsync(sample);
            //await _databaseContext.SaveChangesAsync();
        }
    }
}
