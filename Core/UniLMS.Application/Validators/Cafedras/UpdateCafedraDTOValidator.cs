using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Cafedras;

namespace UniLMS.Application.Validators.Cafedras
{
    public class UpdateCafedraDTOValidator : AbstractValidator<UpdateCafedraDTO>
    {
        public UpdateCafedraDTOValidator() 
        {
            RuleFor(i => i.Id)
                .NotEmpty().WithMessage("Yenilənəcək kafedranın Id dəyəri boş ola bilməz!");

            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Kafedra adı boş ola bilməz!")
                .MaximumLength(150).WithMessage("Kafedra adı maksimum 150 simvoldan ibarət ola bilər!");

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage("Kafedra kodu boş ola bilməz!")
                .MaximumLength(20).WithMessage("Kafedra kodu 20 simvoldan çox ola bilməz!");
        }

    }
}
