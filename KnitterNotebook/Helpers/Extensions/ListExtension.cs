using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers.Extensions
{
    public static class ListExtension
    { 
        /// <summary>
        /// Adds item to list if it is not null or empty, based on <see cref="string.IsNullOrWhiteSpace"/>
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
        /// Adds items which are not null or empty to list, based on <see cref="string.IsNullOrWhiteSpace"/>
        /// </summary>
        /// <param name="list">List to update</param>
        /// <param name="items">Items to add</param>
        public static void AddRangeIfNotNullOrEmpty(this List<string> list, IEnumerable<string?> items)
        {
            if (list is null)
            {
                return;
            }
            foreach (string? item in items ?? Enumerable.Empty<string>())
            {
                list.AddIfNotNullOrEmpty(item);
            }
        }
    }
}