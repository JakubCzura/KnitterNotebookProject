using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class Project
    {
        public int Id { get; set; }

        public DateTime? StartDate { get; set; } = null!;

        public DateTime? EndDate { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
