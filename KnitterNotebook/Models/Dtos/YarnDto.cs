using KnitterNotebook.Models.Entities;

namespace KnitterNotebook.Models.Dtos;

public class YarnDto
{
    public YarnDto(Yarn yarn)
    {
        Id = yarn.Id;
        Name = yarn.Name;
    }

    public int Id { get; set; }

    public string Name { get; set; }
}