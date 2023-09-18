using KnitterNotebook.Models.Enums;

namespace KnitterNotebook.Models.Entities;

public class Sample : BaseDbEntity
{
    //Nazwa włóczki
    public string YarnName { get; set; } = string.Empty;

    //Ilość oczek
    public int LoopsQuantity { get; set; }

    //Ilość rzędów
    public int RowsQuantity { get; set; }

    //Rozmiar druta
    public double NeedleSize { get; set; }

    //Jednostka rozmiaru druta
    public NeedleSizeUnit NeedleSizeUnit { get; set; }

    //dodatkowy opis
    public string? Description { get; set; } = null;

    public virtual SampleImage? Image { get; set; } = null;

    public virtual User User { get; set; }

    public int UserId { get; set; }
}