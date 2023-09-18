using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KnitterNotebook.Helpers.Extensions;

public static class EnumerableExtension
{
    /// <exception cref="ArgumentNullException"></exception>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableCollection) => new(enumerableCollection);
}