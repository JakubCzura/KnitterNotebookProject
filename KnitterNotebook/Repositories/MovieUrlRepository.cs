﻿using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;

namespace KnitterNotebook.Repositories
{
    public class MovieUrlRepository : CrudRepository<MovieUrl>, IMovieUrlRepository
    {
        public MovieUrlRepository(KnitterNotebookContext knitterNotebookContext) : base(knitterNotebookContext)
        {
        }
    }
}