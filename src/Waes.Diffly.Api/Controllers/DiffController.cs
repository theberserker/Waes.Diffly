using Microsoft.AspNetCore.Mvc;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Domain;

namespace Waes.Diffly.Api.Controllers
{
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        private readonly IDiffService _service;

        public DiffController(IDiffService service)
        {
            _service = service;    
        }


        /// <summary>
        /// Action that returns the diff for the provided id.
        /// GET v1/diff/{id}
        /// </summary>
        /// <returns>The diff result.</returns>
        [HttpGet("{id:int:min(1)}")]
        public IActionResult Get(int id)
        {
            var diffResult = _service.Diff(id);
            var dto = new DiffResultDto(diffResult.Item1, diffResult.Item2);
            return Ok(dto);
        }

        /// <summary>
        /// Action that creates the instance to diff on the provided side.
        /// POST /v1/diff/{id}/left or POST /v1/diff/{id}/right 
        /// </summary>
        /// <returns>Only HTTP status code.</returns>
        [HttpPost("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Post(int id, DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.Add(id, side, requestDto.EncodedData);
            return Ok();
        }

        /// <summary>
        /// Action that creates or updates the instance to diff on the provided side.
        /// PUT /v1/diff/{id}/left or PUT /v1/diff/{id}/right 
        /// </summary>
        /// <returns>Only HTTP status code.</returns>
        [HttpPut("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Put(int id, DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.AddOrUpdate(id, side, requestDto.EncodedData);
            return Ok();
        }
    }
}

