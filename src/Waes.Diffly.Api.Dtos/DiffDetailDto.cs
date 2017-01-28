using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waes.Diffly.Api.Dtos
{
    /// <summary>
    /// Detail returned in case there is a diff
    /// </summary>
    public class DiffDetailDto
    {
        public int Offset { get; set; }

        public int Length { get; set; }
    }
}
