using CommunityToolkit.Diagnostics;
using KnitterNotebook.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;

namespace KnitterNotebook.Validators
{
    public class UserValidator : IValidator<User>
    {
        //Regex for password:
        //Has minimum 8 characters in length.Adjust it by modifying {6,}
        //At least one uppercase English letter.You can remove this condition by removing (?=.*?[A - Z])
        //At least one lowercase English letter.You can remove this condition by removing (?=.*?[a - z])
        //At least one digit.You can remove this condition by removing (?=.*?[0 - 9])

        private readonly Regex ValidatePasswordRegex = new("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,}$");

        public bool Validate(User user)
        {
            try
            {
                Guard.IsNotNullOrWhiteSpace(user.Nickname);
                Guard.HasSizeLessThanOrEqualTo(user.Nickname, 50);

                Guard.IsNotNullOrWhiteSpace(user.Password);
                Guard.HasSizeLessThanOrEqualTo(user.Password, 50);
                if (!ValidatePasswordRegex.IsMatch(user.Password))
                {
                    throw new ArgumentException("Hasło musi zawierać conajmniej 6 znaków,\nconajmniej jedną literę małą,\nconajmniej jedną literę wielką,\nconajmniej 1 cyfrę");
                }

                if (new EmailAddressAttribute().IsValid(user.Email) == false)
                {
                    throw new ArgumentException("Niepoprawny format adresu e-mail");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
            return true;
        }
    }
}