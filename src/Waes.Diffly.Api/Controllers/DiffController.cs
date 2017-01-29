using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Domain;

namespace Waes.Diffly.Api.Controllers
{
    /// <summary>
    /// The controller providing the functionalities for diffing.
    /// </summary>
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        private readonly IDiffService _service;

        public DiffController(IDiffService service)
        {
            _service = service;    
        }

        /// <summary>
        /// Returns the diff for the provided id.
        /// </summary>
        /// <param name="id">The id of the left and the right side you want to perform diff on.</param>
        /// <returns>The diff result.</returns>
        [HttpGet("{id:int:min(1)}")]
        public IActionResult Get(int id)
        {
            var diffResult = _service.Diff(id);
            var detail = diffResult.Item2.Select(x => new DiffDetailDto { Offset = x.Offset, Length = x.Length }); // TODO: Automapper
            var dto = new DiffResultDto(diffResult.Item1, detail); 
            return Ok(dto);
        }

        /// <summary>
        /// Creates the instance to diff on the provided side.
        /// </summary>
        /// <param name="id">The identifier you are submitting your diff for.</param>
        /// <param name="side">Side of the diff - 'left' or 'right'.</param>
        /// <param name="requestDto">The DTO containing the request data.</param>
        /// <returns>HTTP 201 - Created status code, without content.</returns>
        [HttpPost("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Post(int id, [FromRoute]DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.Add(id, side, requestDto.EncodedData);
            return StatusCode((int)HttpStatusCode.Created);
        }

        /// <summary>
        /// Creates or updates the instance to diff on the provided side.
        /// </summary>
        /// <param name="id">The identifier you are submitting your diff for.</param>
        /// <param name="side">Side of the diff - 'left' or 'right'.</param>
        /// <param name="requestDto">The DTO containing the request data.</param>
        /// <returns>HTTP 201 - Created status code, without content.</returns>
        /// <returns>Only HTTP status code.</returns>
        [HttpPut("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Put(int id, [FromRoute]DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.AddOrUpdate(id, side, requestDto.EncodedData);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}

