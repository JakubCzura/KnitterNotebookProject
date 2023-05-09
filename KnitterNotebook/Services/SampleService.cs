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
        private readonly ISampleRepository _sampleRepository;
        public SampleService(ISampleRepository sampleRepository) : base(sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public async Task CreateAsync(CreateSampleDto createSampleDto)
        {
            Sample sample = new()
            {
                YarnName = createSampleDto.YarnName,
                LoopsQuantity = createSampleDto.LoopsQuantity,
                RowsQuantity = createSampleDto.RowsQuantity,
                NeedleSize = createSampleDto.NeedleSize,
                NeedleSizeUnit = createSampleDto.NeedleSizeUnit,
                Description = createSampleDto.Description,
                User = createSampleDto.User,
                Image = createSampleDto.Image
            };
            await _sampleRepository.CreateAsync(sample);
        }
    }
}
