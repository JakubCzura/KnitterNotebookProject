using KnitterNotebook.Models.Entities;

namespace KnitterNotebook.Models.Dtos;

public class YarnDto(Yarn yarn)
{
    public int Id { get; set; } = yarn.Id;

    public string Name { get; set; } = yarn.Name;
}