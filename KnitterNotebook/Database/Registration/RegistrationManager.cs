using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class RegistrationManager
    {
        private readonly IRegistration _registration;
        private readonly User _user;
        private readonly DatabaseContext _knitterNotebookContext;

        public RegistrationManager(IRegistration registration, User user, DatabaseContext knitterNotebookContext)
        {
            _registration = registration;
            _user = user;
            _knitterNotebookContext = knitterNotebookContext;
        }

        /// <summary>
        /// Registers user
        /// </summary>
        /// <returns></returns>
        public async Task Register()
        {
            await _registration.RegisterUser(_user, _knitterNotebookContext);
        }
    }
}