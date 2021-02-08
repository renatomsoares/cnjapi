using FluentValidation;
using System;
using Domain.Entities;
using System.Text.RegularExpressions;

namespace Services
{
    public class LawsuitValidator : AbstractValidator<Lawsuit>
    {
        public LawsuitValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .OnAnyFailure(x => throw new ArgumentNullException($"The object could not be started."));

            RuleFor(x => x.CaseNumber)
                .NotNull().WithMessage("CaseNumber: Mandatory field.")
                .Must(BeAValidCaseNumber).WithMessage("CaseNumber: Invalid format. The valid format is: NNNNNNN-NN.NNNN.N.NN.NNNN.");
                
            RuleFor(x => x.CourtName)
                .MaximumLength(500).WithMessage("CourtName: The allowed maximum length is 500.")
                .NotNull().WithMessage("CourtName: Mandatory field.");

            RuleFor(x => x.ResponsibleName)
                .MaximumLength(500).WithMessage("ResponsibleName: The allowed maximum length is 500.")
                .NotNull().WithMessage("ResponsibleName: Mandatory field.");
        }

        private bool BeAValidCaseNumber(string caseNumber)
        {
            return caseNumber == null ? true : Regex.IsMatch(caseNumber, @"(^(\d{7}-\d{2}.\d{4}.\d{1}.\d{2}.\d{4})|(\d{25})$)");
        }
    }
}
