using KnitterNotebook.Models;
using System.Threading.Tasks;

namespace KnitterNotebook.Database.Registration
{
    public class RegistrationManager
    {
        private IRegistration Registration { get; set; }
        private User User { get; set; }

        private KnitterNotebookContext KnitterNotebookContext { get; set; }

        public RegistrationManager(IRegistration registration, User user, KnitterNotebookContext knitterNotebookContext)
        {
            Registration = registration;
            User = user;
            KnitterNotebookContext = knitterNotebookContext;
        }

        /// <summary>
        /// Registers user
        /// </summary>
        /// <returns></returns>
        public async Task Register()
        {
            await Registration.RegisterUser(User, KnitterNotebookContext);
        }
    }
}