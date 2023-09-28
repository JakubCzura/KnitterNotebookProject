using KnitterNotebook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Converters;

public class CreateYarnDtoConverter
{
    /// <summary>
    /// Converts <paramref name="yarnsNames"/> to IEnumerable of <see cref="CreateYarnDto"/>.
    /// </summary>
    /// <param name="yarnsNames">Yarns names to convert</param>
    /// <param name="delimiter">Delimiter to separate each yarn's name</param>
    /// <returns>IEnumerable of converted <seealso cref="CreateYarnDto"/> for valid <paramref name="yarnsNames"/>, otherwise <see cref="Enumerable.Empty{CreateYarnDto}"/></returns>
    public static IEnumerable<CreateYarnDto> Convert(string yarnsNames, char delimiter = ',')
        => yarnsNames?.Split(delimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                     .Select(x => new CreateYarnDto(x.Trim()))
                     ?? Enumerable.Empty<CreateYarnDto>();
}