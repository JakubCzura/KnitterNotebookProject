using KnitterNotebook.Database;
using KnitterNotebook.Models;
using KnitterNotebook.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KnitterNotebookContext _knitterNotebookContext;

        public UserRepository(KnitterNotebookContext knitterNotebookContext)
        {
            _knitterNotebookContext = knitterNotebookContext;
        }

        public async Task Add(User user)
        {
            await _knitterNotebookContext.Users.AddAsync(user);
            await _knitterNotebookContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            User? user = await _knitterNotebookContext.Users.FindAsync(id);
            _knitterNotebookContext.Users.Remove(user!);
            await _knitterNotebookContext.SaveChangesAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _knitterNotebookContext.Users.FindAsync(id) ?? null!;
        }

        public async Task<List<User>> GetAll()
        {
            return await _knitterNotebookContext.Users.ToListAsync();
        }

        public async Task Update(User user)
        {
            _knitterNotebookContext.Users.Update(user);
            await _knitterNotebookContext.SaveChangesAsync();
        }
    }
}
