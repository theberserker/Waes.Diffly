using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Waes.Diffly.Api.Dtos
{
    public class DiffRequestDto
    {
        public DiffRequestDto()
        {
        }

        public DiffRequestDto(string data)
        {
            this.EncodedData = data;
        }

        public string EncodedData { get; set; }
    }
}
