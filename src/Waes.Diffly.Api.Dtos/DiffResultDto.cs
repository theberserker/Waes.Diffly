using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Waes.Diffly.Api.Dtos.Enums;

namespace Waes.Diffly.Api.Dtos
{
    /// <summary>
    /// Result object of the diff operation.
    /// </summary>
    public class DiffResultDto
    {
        public DiffResultDto()
        {
        }

        public DiffResultDto(DiffResultType result, IEnumerable<int> diffs) : this()
        {
            this.Result = result;
            this.Diffs = diffs;
        }

        /// <summary>
        /// Result enum of the diff operation.
        /// </summary>
        public DiffResultType Result { get; set; }

        /// <summary>
        /// Indexes that differ in the diff operation.
        /// </summary>
        public IEnumerable<int> Diffs { get; set; }
    }
}
