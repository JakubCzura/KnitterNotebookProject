using KnitterNotebook.Models.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public class NeedlesToPlanProjectFilter
    {
        public static IEnumerable<CreateNeedleDto> GetNeedlesWithSizeHasValue(params CreateNeedleDto[] needles)
            => needles.Where(x => x.Size.HasValue);
    }
}