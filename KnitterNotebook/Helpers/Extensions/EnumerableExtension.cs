using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions;

public static class EnumerableExtension
{
    /// <summary>
    /// Converts <paramref name="collection"/> to <see cref="ObservableCollection{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of elements in collection</typeparam>
    /// <param name="collection">Collection to convert</param>
    /// <returns>New instance of <see cref="ObservableCollection{T}"/></returns>
    /// <exception cref="ArgumentNullException">When <paramref name="collection"/> is null</exception>
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> collection)
        => new(collection);

    /// <summary>
    /// Checks if <paramref name="collection"/> is not null and has any element.
    /// </summary>
    /// <typeparam name="T">Type of elements in collection</typeparam>
    /// <param name="collection">Collection to check</param>
    /// <returns>True if <paramref name="collection"/> is not null and have any element, otherwise false</returns>
    public static bool NotNullAndHaveAnyElement<T>(this IEnumerable<T> collection)
        => collection is not null && collection.Any();
}