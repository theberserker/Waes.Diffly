using System;
using Microsoft.AspNetCore.Mvc;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;

namespace Waes.Diffly.Api.Controllers
{
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        private readonly IDiffRepository _repository;

        public DiffController(IDiffRepository diffRepository)
        {
            _repository = diffRepository;    
        }

        /// <summary>
        /// Action that returns the diff for the provided id.
        /// GET v1/diff/{id}
        /// </summary>
        /// <returns>The diff result</returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{id}/{side}")]
        public IActionResult Post(int id, DiffSide side, [FromBody]DiffRequestDto value)
        {
            if (side == DiffSide.Unknown)
            {
                throw new NotSupportedException($"Side provided is not supported. Side should be '{DiffSide.Left.ToString().ToLower()}' or '{DiffSide.Right.ToString().ToLower()}'.");
            }

            return Ok();
        }
    }
}

