using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Services.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAll();

        public Task<User> Get(int id);

        public Task Add(RegisterUserDto registerUserDto);

        public Task Update(User user);

        public Task Delete(int id);
    }
}
