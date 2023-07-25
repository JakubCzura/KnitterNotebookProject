using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnitterNotebook.Converters
{
    public class CreateYarnDtoConverter
    {
        public static IEnumerable<CreateYarnDto> Convert(string yarnsNames, char delimiter = ',')
        {
            yarnsNames = yarnsNames.TrimEnd();

            if(yarnsNames.EndsWith(delimiter))
            {
                yarnsNames = yarnsNames[..^1];
            }

            return yarnsNames.Split(delimiter).Select(x => new CreateYarnDto(x));
        }
    }
}
