using FluentValidation;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;

namespace UniLMS.Application.Validators.Attendances
{
    public class CreateAttendanceRangeDTOValidator : AbstractValidator<CreateAttendanceRangeDTO>
    {
        public CreateAttendanceRangeDTOValidator() 
        {
            RuleFor(x => x.CourseScheduleId)
                .NotEmpty().WithMessage("Dərs cədvəli seçilməlidir.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Tarix qeyd olunmalıdır.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Gələcək tarix üçün davamiyyət qeyd edilə bilməz.");

            RuleFor(x => x.Students)
                .NotNull().WithMessage("Tələbə siyahısı göndərilməlidir.")
                .Must(x => x != null && x.Count > 0).WithMessage("Davamiyyət üçün ən azı bir tələbə olmalıdır.");

            RuleForEach(x => x.Students)
                .SetValidator(new CreateStudentAttendanceDTOValidator());
        }
    }
}
