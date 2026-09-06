using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Faculties;

namespace UniLMS.Application.Validators.Faculties
{
    public class CreateFacultyDTOValidator : AbstractValidator<CreateFacultyDTO>
    {
        public CreateFacultyDTOValidator()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("Fakültə adı boş ola bilməz!")
                .MaximumLength(150).WithMessage("Fakültə adı maximum 150 simvoldan ibarət olmalıdır!");

            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Fakültə kodu boş ola bilməz!")
                .MaximumLength(20).WithMessage("Fakültə kodu maximum 150 simvoldan ibarət olmalıdır!");
                
        }
    }
}
