using System;
using Microsoft.AspNetCore.Mvc;
using Waes.Diffly.Api.Dtos;
using Waes.Diffly.Core.Domain.Enums;
using Waes.Diffly.Core.Interfaces.Repositories;
using Waes.Diffly.Core.Interfaces.Domain;
using Waes.Diffly.Core.Domain.Entities;

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
        /// Dummy controller for now, in order to see the app running.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello world!");
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
            _service.Add(id, side, value.EncodedData);
            return Ok();
        }

        //[HttpPut("{id}/{side}")]
        //public IActionResult Put(int id, DiffSide side, [FromBody]DiffRequestDto value)
        //{
        //    _service.AddOrUpdate(id, side, value.EncodedData);
        //    return Ok();
        //}
    }
}

