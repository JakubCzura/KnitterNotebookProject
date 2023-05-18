using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface ISampleService : ICrudService<Sample>
    {
        Task CreateAsync(CreateSampleDto createSampleDto);
    }
}
