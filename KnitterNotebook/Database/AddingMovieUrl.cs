using KnitterNotebook.Database.Interfaces;
using KnitterNotebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database
{
    public class AddingMovieUrl : IAddingMovieUrl
    {
        public async Task<bool> AddMovieUrl(MovieUrl movieUrl, KnitterNotebookContext knitterNotebookContext)
        {
          //  try
          //  {
                await knitterNotebookContext.AddAsync(movieUrl);
                await knitterNotebookContext.SaveChangesAsync();
                return true;
          //  }
           // catch
           // {
           //     return false;
           // }
        }
    }
}
