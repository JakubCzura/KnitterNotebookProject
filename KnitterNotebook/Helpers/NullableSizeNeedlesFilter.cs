using KnitterNotebook.Models;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public class NullableSizeNeedlesFilter
    {
        public static IEnumerable<NullableSizeNeedle> GetNeedlesWithSizeHasValue(params NullableSizeNeedle[] needles)
            => needles.Where(x => x.Size.HasValue);
    }
}