using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.ExamResults;

namespace UniLMS.Application.Validators.ExamResults
{
    public class CreateExamResultRangeDTOValidator : AbstractValidator<CreateExamResultRangeDTO>
    {
        public CreateExamResultRangeDTOValidator() 
        {
            RuleFor(x => x.CourseOfferingId)
                .NotEmpty().WithMessage("Dərs qeyd İd mütləq daxil edilməlidir");

            RuleFor(x => x.ExamType)
                .IsInEnum().WithMessage("İmtahan növü mütləq qeyd olunmalıdır");

            RuleFor(x => x.EvaluationDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Gələcək tarix üçün imtahan qeyd edilə bilməz.")
                .WithMessage("Tarix mütləq qeyd olunmalıdır");
                
            RuleFor(x => x.StudentScores)
             .NotEmpty().WithMessage("Ən azı bir tələbə üçün bal daxil edilməlidir.")
             .Must(x => x != null && x.Count > 0).WithMessage("Tələbə siyahısı boş ola bilməz.");

            
            RuleForEach(x => x.StudentScores)
                .SetValidator(new CreateStudentExamResultDTOValidator());
        }
    }
}
