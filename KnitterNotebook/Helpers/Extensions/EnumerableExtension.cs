using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions;

public static class EnumerableExtension
{
    /// <exception cref="ArgumentNullException"></exception>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableCollection) => new(enumerableCollection);

    public static bool NotNullAndHaveAnyElement<T>(this IEnumerable<T> enumerableCollection) => enumerableCollection is not null && enumerableCollection.Any();
}