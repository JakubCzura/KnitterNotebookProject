using KnitterNotebook.Models.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace KnitterNotebook.Helpers
{
    public class NeedlesToPlanProjectFilter
    {
        public static IEnumerable<NeedleToPlanProjectDto> GetNeedlesWithSizeHasValue(params NeedleToPlanProjectDto[] needles)
            => needles.Where(x => x.Size.HasValue);
    }
}