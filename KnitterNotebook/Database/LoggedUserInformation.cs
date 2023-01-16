using KnitterNotebook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Database
{
    public class LoggedUserInformation
    {
        public static User LoggedUser { get; set; } = null!;
    }
}
