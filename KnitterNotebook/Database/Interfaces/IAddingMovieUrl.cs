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
        public Task AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext);
    }
}
