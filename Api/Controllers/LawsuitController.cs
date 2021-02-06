using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Application.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Lawsuit Controller
    /// </summary>
    [Route("[controller]")]
    public class LawsuitController : Controller
    {
        /// <summary>
        /// Get all lawsuits.
        /// </summary>
        /// <response code="200">list returned all lawsuits</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet]
        public IActionResult GetAll([FromServices] LawsuitService service)
        {
            var lawsuits = service.GetAll().ToList();
            var response = Mapper.Map<List<Lawsuit>, List<LawsuitDTO>>(lawsuits);

            if (!response.Any())
                return NotFound();

            return Ok(response);
        }

        /// <summary>
        /// Update lawsuit by Id.
        /// </summary>
        /// <response code="200">return lawsuit</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>       
        [HttpGet("{id}")]
        public IActionResult GetById([FromServices] LawsuitService service, int id)
        {
            HttpContext.Items.TryGetValue("token", out service._tokenInfo);
            var lawsuit = service.GetById(x => x.IdLawsuit == id);
            var response = Mapper.Map<Lawsuit, Lawsuit>(lawsuit);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        /// <summary>
        /// Create new Lawsuit.
        /// </summary>
        /// <param name="lawsuitDto"></param>
        /// <response code="201">created</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="500">internal error server</response>
        [HttpPost]
        public IActionResult Create([FromServices] LawsuitService service, [FromBody] LawsuitDTO lawsuitDto)
        {

            var lawsuit = Mapper.Map<Lawsuit>(lawsuitDto);
            var response = service.Add<LawsuitValidator>(lawsuit);

            return new ObjectResult(new { success = true, data = response });
        }


        /*
        /// <summary>
        /// Atualiza TemplateRelatorioAnual, utilizando o objeto TemplateRelatorioAnualDTO e o Id do TemplateRelatorioAnual.
        /// </summary>
        /// <param name="templateRelatorioAnualDto"></param>
        /// <param name="id">Id do TemplateRelatorioAnual</param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPut("{id}")]
        public IActionResult Update([FromServices] LawsuitService service, [FromBody] LawsuitDTO templateRelatorioAnualDto, int id)
        {
            HttpContext.Items.TryGetValue("token", out service._tokenInfo);
            var templateRelatorioAnualRepo = service.GetById(x => x.IdTemplateRelatorioAnual == id);

            if (templateRelatorioAnualRepo == null)
                return NotFound();

            service.ValidateAno(templateRelatorioAnualDto);
            var mapped = Mapper.Map(templateRelatorioAnualDto, templateRelatorioAnualRepo);
            var result = service.Update<LawsuitValidator>(mapped);
            var templateRelatorioAnualDTO = Mapper.Map<TemplateRelatorioAnual, LawsuitDTO>(result);

            return Ok(templateRelatorioAnualDTO);
        }

        /// <summary>
        /// Deleta TemplateRelatorioAnual, utilizando o Id do TemplateRelatorioAnual.
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
            HttpContext.Items.TryGetValue("token", out service._tokenInfo);
            var templateRelatorioAnualRepo = service.GetById(x => x.IdTemplateRelatorioAnual == id);

            if (templateRelatorioAnualRepo == null)
                return NotFound();

            service.ValidateRelatorio(templateRelatorioAnualRepo);
            var mapped = Mapper.Map<TemplateRelatorioAnual>(templateRelatorioAnualRepo);
            service.Delete(mapped);

            return new NoContentResult();
        }

        /// <summary>
        /// Gera um PDF do TemplateRelatorioAnual, utilizando o objeto TemplateRelatorioAnualDTO.
        /// </summary>
        /// <param name="templateRelatorioAnualDto"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPost("pdf")]
        public IActionResult GerarPdfTemplate([FromServices] LawsuitService service, [FromBody] LawsuitDTO templateRelatorioAnualDto)
        {
            HttpContext.Items.TryGetValue("token", out service._tokenInfo);

            var result = service.GerarPdf(templateRelatorioAnualDto);

            return Ok(result);
        }

        /// <summary>
        /// Upload a TemplateDoc, utilizando o Id do TemplateRelatorioAnual e o arquivo doc.
        /// </summary>
        /// <param name="templateDoc"></param>
        /// <param name="id"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPut("doc/{id}")]
        public IActionResult UploadTemplateDoc([FromServices] LawsuitService service, [FromForm] IFormFile templateDoc, int id)
        {
            var repositorio = service.GetById(x => x.IdTemplateRelatorioAnual == id);

            service.UploadTemplateDoc<LawsuitValidator>(repositorio, templateDoc);

            //Retornos vazios são NotFound
            if (repositorio == null)
                return NotFound();

            return Ok(repositorio);
        }

        /// <summary>
        /// Retorna o Arquivo do TemplateRelatorioAnual
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201">created</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="500">internal error server</response>
        [HttpGet("download/{id}")]
        public async Task<FileStream> DownloadTemplateDoc([FromServices] LawsuitService service, int id)
        {
            var templateRelatorioAnualRepo = service.GetAll(x => x.IdTemplateRelatorioAnual == id).FirstOrDefault();

            if (templateRelatorioAnualRepo == null)
                throw new FileNotFoundException();

            var result = await service.DownloadTemplateDoc(templateRelatorioAnualRepo);

            return result;
        }
        */
    }
}
