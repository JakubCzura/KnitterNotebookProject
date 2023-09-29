using KnitterNotebook.Models;
using KnitterNotebook.Models.Dtos;
using System;

namespace KnitterNotebook.Converters;

public class CreateNeedleDtoConverter
{
    /// <summary>
    /// Converts <paramref name="nullableSizeNeedle"/> to new instance of <see cref="CreateNeedleDto"/>
    /// </summary>
    /// <param name="nullableSizeNeedle">Object to convert</param>
    /// <returns>New instance of <see cref="CreateNeedleDto"/></returns>
    /// <exception cref="NullReferenceException"></exception>"
    public static CreateNeedleDto Convert(NullableSizeNeedle nullableSizeNeedle) => new(System.Convert.ToDouble(nullableSizeNeedle.Size), nullableSizeNeedle.SizeUnit);
}