namespace KnitterNotebook.Models
{
    public class Sample
    {
        public Sample()
        {
        }

        public Sample(int id, string yarnName, int loopsQuantity, int rowsQuantity, int needleSize, string needleSizeUnit, string description, User user)
        {
            Id = id;
            YarnName = yarnName;
            LoopsQuantity = loopsQuantity;
            RowsQuantity = rowsQuantity;
            NeedleSize = needleSize;
            NeedleSizeUnit = needleSizeUnit;
            User = user;
            Description = description;
        }

        public int Id { get; set; }

        //Nazwa włóczki
        public string YarnName { get; set; } = string.Empty;

        //Ilość oczek
        public int LoopsQuantity { get; set; }

        //Ilość rzędów
        public int RowsQuantity { get; set; }

        //Rozmiar druta
        public int NeedleSize { get; set; }

        //Jednostka rozmiaru druta
        public string NeedleSizeUnit { get; set; } = string.Empty;

        //dodatkowy opis
        public string Description { get; set; } = string.Empty;

        public User User { get; set; } = new();
    }
}