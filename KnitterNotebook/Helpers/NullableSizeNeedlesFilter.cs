using KnitterNotebook.Models;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public class NullableSizeNeedlesFilter
    {
        /// <summary>
        /// Filter needles collection if needle's size is greater than 0 
        /// </summary>
        /// <param name="needles">Needles to filter</param>
        /// <returns>Collection of needles with size greater than 0</returns>
        public static IEnumerable<NullableSizeNeedle> GetNeedlesWithPositiveSizeValue(params NullableSizeNeedle[] needles)
            => needles.Where(x => x.Size.HasValue && x.Size > 0);
    }
}