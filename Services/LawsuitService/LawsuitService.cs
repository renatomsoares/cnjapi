using System.Collections.Generic;
using FluentValidation;
using System.Linq;
using Domain.DTO;
using Domain.Entities;
using Infra.UnitOfWork;
using Services._BaseService;

namespace Services
{
    public class LawsuitService : BaseService<Lawsuit>
    {
        private readonly IUnitOfWork _uow;

        public LawsuitService(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
        }

        public void LawsuitPrevalidations(LawsuitDTO lawsuitDto) 
        {
            if (lawsuitDto != null)
            {

            // 1 - Number case exists
            List<Lawsuit> lawsuits = GetAll(x => x.CaseNumber == lawsuitDto.CaseNumber).ToList();

            if (lawsuits.Count > 0)
                throw new ValidationException("Case number exists.");

            }
        }

        public List<Lawsuit> GetAllFilter(LawsuitDTO lawsuitDto)
        {
            List<Lawsuit> result = new List<Lawsuit>();
            result = GetAll(x => (x.CaseNumber.Equals(lawsuitDto.CaseNumber) || string.IsNullOrEmpty(lawsuitDto.CaseNumber))
                             && (x.CourtName.Contains(lawsuitDto.CourtName) || string.IsNullOrEmpty(lawsuitDto.CourtName))
                             && (x.ResponsibleName.Contains(lawsuitDto.ResponsibleName) || string.IsNullOrEmpty(lawsuitDto.ResponsibleName))).ToList();

            return result;
        }

    }
}
