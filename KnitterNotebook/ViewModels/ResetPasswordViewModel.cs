using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KnitterNotebook.ViewModels
{
  
    public class ResetPasswordViewModel : BaseViewModel
    {
        public ResetPasswordViewModel()
        {
            ResetPasswordCommandAsync = new AsyncRelayCommand(ResetPasswordAsync);
        }

        public ICommand ResetPasswordCommandAsync { get; }

        private string _emailOrNickname = string.Empty; 
        public string EmailOrNickname
        {
            get { return _emailOrNickname; }
            set { _emailOrNickname = value; OnPropertyChanged(); }
        }

        public async Task ResetPasswordAsync()
        {

        }
    }
}
