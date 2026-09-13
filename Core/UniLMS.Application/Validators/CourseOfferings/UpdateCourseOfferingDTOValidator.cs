using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.CourseOfferings;

namespace UniLMS.Application.Validators.CourseOfferings
{
    public class UpdateCourseOfferingDTOValidator : AbstractValidator<UpdateCourseOfferingDTO>
    {
        public UpdateCourseOfferingDTOValidator() 
        {
            RuleFor(i => i.Id)
                .NotEmpty().WithMessage("Id dəyəri boş ola bilməz!");

            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage("Semestr seçilməlidir.");

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage("Fənn seçilməlidir.");

            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("Qrup seçilməlidir.");

            RuleFor(x => x.TeacherId)
                .NotEmpty().WithMessage("Müəllim seçilməlidir.");

            RuleFor(x => x.RoomCode)
                .NotEmpty().WithMessage("Otaq kodu daxil edilməlidir.")
                .MaximumLength(50).WithMessage("Otaq kodu maksimum 50 simvol ola bilər.");
        }
    }
}
