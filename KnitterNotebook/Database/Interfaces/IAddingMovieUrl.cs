using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Interfaces
{
    interface IAddingMovieUrl
    {
        public Task <bool> AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext);
    }
}
