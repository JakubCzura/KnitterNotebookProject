namespace KnitterNotebook.Models
{
    public class Sample
    {
        public Sample()
        {
        }

        public Sample(int id, string yarnName, int loopsQuantity, int rowsQuantity, int needleSize, string needleSizeUnit, string description, User user, Image? image = null)
        {
            Id = id;
            YarnName = yarnName;
            LoopsQuantity = loopsQuantity;
            RowsQuantity = rowsQuantity;
            NeedleSize = needleSize;
            NeedleSizeUnit = needleSizeUnit;
            Description = description;
            User = user;
            Image = image;
        }

        public int Id { get; set; }

        //Nazwa włóczki
        public string YarnName { get; set; } = string.Empty;

        //Ilość oczek
        public int LoopsQuantity { get; set; }

        //Ilość rzędów
        public int RowsQuantity { get; set; }

        //Rozmiar druta
        public double NeedleSize { get; set; }

        //Jednostka rozmiaru druta
        public string NeedleSizeUnit { get; set; } = string.Empty;

        //dodatkowy opis
        public string? Description { get; set; } = null;

        public virtual Image? Image { get; set; } = null;

        public int? ImageId { get; set; }

        public virtual User User { get; set; } = new();
    }
}