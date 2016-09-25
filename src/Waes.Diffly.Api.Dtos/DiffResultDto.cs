using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waes.Diffly.Api.Dtos.Enums;

namespace Waes.Diffly.Api.Dtos
{
    public class DiffResultDto
    {
        public DiffResultDto(DiffResultType result)
        {
            this.Result = result;
        }

        public DiffResultDto(DiffResultType result, IEnumerable<int> diffs) : this(result)
        {
            this.Diffs = diffs;
        }

        public DiffResultType Result { get; set; }
        public IEnumerable<int> Diffs { get; set; }
    }
}
