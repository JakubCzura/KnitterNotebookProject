﻿using KnitterNotebook.Database;
using KnitterNotebook.Models;

namespace KnitterNotebook.Repositories.Interfaces
{
    public class SampleRepository : CrudRepository<Sample>, ISampleRepository
    {
        public SampleRepository(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
    }
}