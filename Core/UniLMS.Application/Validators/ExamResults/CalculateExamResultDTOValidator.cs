using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;

namespace UniLMS.Application.Validators.ExamResults
{
    public class CalculateExamResultDTOValidator : AbstractValidator<CalculateExamResultDTO>
    {
        public CalculateExamResultDTOValidator() 
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Tələbə İD-si boş ola bilməz.");

            RuleFor(x => x.CourseOfferingId)
                .NotEmpty().WithMessage("Dərs İD-si boş ola bilməz.");
        }
    }
}
