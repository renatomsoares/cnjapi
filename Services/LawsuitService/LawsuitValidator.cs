using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
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
                .Must(BeAValidCaseNumber);

            RuleFor(x => x.CourtName)
                .MaximumLength(500)
                .NotEmpty();

            RuleFor(x => x.ResponsibleName)
                .MaximumLength(500)
                .NotEmpty();
        }

        private bool BeAValidCaseNumber(string caseNumber)
        {
            return Regex.IsMatch(caseNumber, @"(^(\d{7}-\d{2}.\d{4}.\d{1}.\d{2}.\d{4})|(\d{25})$)");
        }
    }
}
