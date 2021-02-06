using System;
using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using System.Text;
using Domain.DTO;
using Domain.Entities;
using Infra.UnitOfWork;
using Services._BaseService;
using Microsoft.Extensions.Configuration;
using System.IO;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Services
{
    public class LawsuitService : BaseService<Lawsuit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        public LawsuitService(IUnitOfWork uow, IConfiguration configuration) : base(uow)
        {
            _uow = uow;
            _configuration = configuration;
        }

        public void LawsuitPrevalidations(LawsuitDTO lawsuitDto) 
        {
            // 1 - Number case exists
            List<Lawsuit> lawsuits = GetAll(x => x.CaseNumber == lawsuitDto.CaseNumber).ToList();

            if (lawsuits.Count > 0)
                throw new ValidationException("Case number exists.");
        }
    }
}
