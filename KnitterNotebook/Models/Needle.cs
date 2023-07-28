namespace KnitterNotebook.Models
{
    public class Needle
    {
        public Needle()
        {
        }

        public Needle(double size, string sizeUnit)
        {
            Size = size;
            SizeUnit = sizeUnit;
        }

        public int Id { get; set; }

        public double Size { get; set; }

        public string SizeUnit { get; set; } = string.Empty;

        public Project Project { get; set; } = new();
    }
}