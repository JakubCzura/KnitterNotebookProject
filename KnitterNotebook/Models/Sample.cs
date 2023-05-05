using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Models
{
    public class Sample
    {
        public Sample()
        {
        }

        public Sample(int id, string yarnName, int mashesQuantity, int rowsQuantity, int needleSize, string needleSizeUnit, User user)
        {
            Id = id;
            YarnName = yarnName;
            MashesQuantity = mashesQuantity;
            RowsQuantity = rowsQuantity;
            NeedleSize = needleSize;
            NeedleSizeUnit = needleSizeUnit;
            User = user;
        }

        public int Id { get; set; }

        //Nazwa włóczki
        public string YarnName { get; set; } = string.Empty;

        //Ilość oczek
        public int MashesQuantity { get; set; }

        //Ilość rzędów
        public int RowsQuantity { get; set; }

        //Rozmiar druta
        public int NeedleSize { get; set; }

        //Jednostka rozmiaru druta
        public string NeedleSizeUnit { get; set; } = string.Empty;

        public User User { get; set; } = new();
    }
}
