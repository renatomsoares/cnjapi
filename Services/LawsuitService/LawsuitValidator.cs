using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;


namespace Services
{
    public class LawsuitValidator : AbstractValidator<Lawsuit>
    {
        public LawsuitValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x => throw new ArgumentNullException($"The object could not be started."));
        }
    }
}
