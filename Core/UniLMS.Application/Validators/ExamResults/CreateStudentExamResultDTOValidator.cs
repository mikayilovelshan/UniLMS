using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;

namespace UniLMS.Application.Validators.ExamResults
{
    public class CreateStudentExamResultDTOValidator : AbstractValidator<CreateStudentExamResultDTO>
    {
        public CreateStudentExamResultDTOValidator() 
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Tələbə ID-si boş ola bilməz.");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Tələbənin balı 0 ilə 100 arasında olmalıdır.");
        }
    }
}
