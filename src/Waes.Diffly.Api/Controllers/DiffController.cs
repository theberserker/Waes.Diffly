﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
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
        /// Returns the diff for the provided id.
        /// </summary>
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
        /// <returns>Only HTTP status code.</returns>
        [HttpPost("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Post(int id, DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.Add(id, side, requestDto.EncodedData);
            return StatusCode((int)HttpStatusCode.Created);
        }

        /// <summary>
        /// Creates or updates the instance to diff on the provided side.
        /// </summary>
        /// <returns>Only HTTP status code.</returns>
        [HttpPut("{id:int:min(1)}/{side:DiffSide}")]
        public IActionResult Put(int id, DiffSide side, [FromBody]DiffRequestDto requestDto)
        {
            _service.AddOrUpdate(id, side, requestDto.EncodedData);
            return StatusCode((int)HttpStatusCode.Created);
        }
    }
}

