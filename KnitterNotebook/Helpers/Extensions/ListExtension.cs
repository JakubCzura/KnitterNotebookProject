using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions
{
    public static class ListExtension
    { 
        /// <summary>
        /// Adds <paramref name="item"/> to <paramref name="list"/> if the item is not null or empty and the list is not null, based on <see cref="string.IsNullOrWhiteSpace"/>
        /// </summary>
        /// <param name="list">List to update</param>
        /// <param name="item">Item to add</param>
        public static void AddIfNotNullOrEmpty(this List<string> list, string? item)
        {
            if (list is null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(item))
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Adds <paramref name="items"/> which are not null or empty to <paramref name="list"/> if the list is not null, based on <see cref="string.IsNullOrWhiteSpace"/>
        /// </summary>
        /// <param name="list">List to update</param>
        /// <param name="items">Items to add</param>
        public static void AddRangeIfNotNullOrEmpty(this List<string> list, IEnumerable<string?> items)
        {
            foreach (string? item in items ?? Enumerable.Empty<string>())
            {
                list.AddIfNotNullOrEmpty(item);
            }
        }
    }
}