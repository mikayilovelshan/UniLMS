using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;

namespace UniLMS.Application.Validators.ExamResults
{
    public class UpdateExamResultDTOValidator : AbstractValidator<UpdateExamResultDTO>
    {
        public UpdateExamResultDTOValidator() 
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("İd daxil edilməlidir");

            RuleFor(x => x.EvaluationDate)
                .NotEmpty().WithMessage("Tarix mütləq qeyd olunmalıdır")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Gələcək tarix üçün imtahan qeyd edilə bilməz.");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Tələbənin balı 0 ilə 100 arasında olmalıdır.");
        }
    }
}
