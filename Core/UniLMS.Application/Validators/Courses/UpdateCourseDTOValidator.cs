using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Courses;

namespace UniLMS.Application.Validators.Courses
{
    public  class UpdateCourseDTOValidator : AbstractValidator<UpdateCourseDTO>
    {
        public UpdateCourseDTOValidator() 
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("Kursun İd dəyəri boş qala bilməz");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Kurs adı boş qala bilməz")
                .MaximumLength(150).WithMessage("Kurs adı 150 simvoldan çox ola bilməz");

            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Kurs kodu boş qala bilməz")
                .MaximumLength(20).WithMessage("Kurs kodu 20 simvoldan çox olmamalıdır");
        }
    }
}
