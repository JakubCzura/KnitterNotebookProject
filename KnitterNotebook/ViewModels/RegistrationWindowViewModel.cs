using KnitterNotebook.Models;
using KnitterNotebook.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.ViewModels
{
    public class RegistrationWindowViewModel : BaseViewModel
    {
        private string nickname;

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; OnPropertyChanged(); }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; OnPropertyChanged(); }
        }

        public RegistrationWindowViewModel()
        {
            
        }
    }
}
