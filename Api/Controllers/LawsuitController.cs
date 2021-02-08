using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Views;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Lawsuit controller class.
    /// </summary>
    [Route("[controller]")]
    public class LawsuitController : Controller
    {
        /// <summary>
        /// Get all lawsuits.
        /// </summary>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet]
        public IActionResult GetAll([FromServices] LawsuitService service)
        {
            List<Lawsuit> lawsuits = service.GetAll().ToList();
            var response = Mapper.Map<List<Lawsuit>, List<LawsuitView>>(lawsuits);

            if (!response.Any())
                return NoContent();

            return Ok(response);
        }

        /// <summary>
        /// Update lawsuit by id.
        /// </summary>
        /// <param name="id">Lawsuit id</param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>   
        [HttpGet("{id}")]
        public IActionResult GetById([FromServices] LawsuitService service, int id)
        {
            Lawsuit lawsuit = service.GetById(x => x.IdLawsuit == id);

            if (lawsuit == null)
                return NotFound();

            var response = Mapper.Map<Lawsuit, LawsuitView>(lawsuit);

            return Ok(response);
        }

        /// <summary>
        /// Create a new lawsuit.
        /// </summary>
        /// <param name="lawsuitDto"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPost]
        public IActionResult Create([FromServices] LawsuitService service, [FromBody] LawsuitDTO lawsuitDto)
        {
            service.LawsuitPrevalidations(lawsuitDto);

            Lawsuit lawsuit = Mapper.Map<Lawsuit>(lawsuitDto);
            var createdLawsuit = service.Add<LawsuitValidator>(lawsuit);
            var response = Mapper.Map<Lawsuit, LawsuitView>(createdLawsuit);

            if (response == null)
            {
                return BadRequest();

            } else
            {
                return Ok(response);
            }
        }


        /// <summary>
        /// Update a lawsuit.
        /// </summary>
        /// <param name="lawsuitDto"></param>
        /// <param name="id">Lawsuit id</param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPut("{id}")]
        public IActionResult Update([FromServices] LawsuitService service, [FromBody] LawsuitDTO lawsuitDto, int id)
        {
            Lawsuit lawsuit = service.GetById(x => x.IdLawsuit == id);

            if (lawsuit == null)
                return NotFound();

            var mappedLawsuit = Mapper.Map(lawsuitDto, lawsuit);
            var updatedLawsuit = service.Update<LawsuitValidator>(mappedLawsuit);
            var response = Mapper.Map<Lawsuit, LawsuitView>(updatedLawsuit);

            return Ok(response);
        }

        /// <summary>
        /// Delete a lawsuit.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromServices] LawsuitService service, int id)
        {
            var lawsuit = service.GetById(x => x.IdLawsuit == id);

            if (lawsuit == null)
                return NotFound();

            var mapped = Mapper.Map<Lawsuit>(lawsuit);
            service.Delete(mapped);

            return Ok();
        }
    }
}
