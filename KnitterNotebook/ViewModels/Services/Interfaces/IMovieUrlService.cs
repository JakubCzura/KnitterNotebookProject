using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels.Services.Interfaces
{
    public interface IMovieUrlService
    {
        Task DeleteMovieUrlAsync(MovieUrl movieUrl);

        Task AddMovieUrlAsync(MovieUrl movieUrl);
    }
}
