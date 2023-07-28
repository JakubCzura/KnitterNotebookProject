using System.Collections.Generic;

namespace KnitterNotebook.Models.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
        }

        public UserDto(int id, string nickname, string email, List<Project> projects, List<Sample> samples, List<MovieUrl> movieUrls, Theme? theme)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
            Projects = projects;
            Samples = samples;
            MovieUrls = movieUrls;
            Theme = theme;
        }

        public int Id { get; set; }

        public string Nickname { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public List<Project> Projects { get; set; } = new();

        public List<Sample> Samples { get; set; } = new();

        public List<MovieUrl> MovieUrls { get; set; } = new();

        public Theme? Theme { get; set; } = null;
    }
}