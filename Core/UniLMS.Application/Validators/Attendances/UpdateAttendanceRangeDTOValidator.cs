using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;

namespace UniLMS.Application.Validators.Attendances
{
    public class UpdateAttendanceRangeDTOValidator : AbstractValidator<UpdateAttendanceRangeDTO>
    {
        public UpdateAttendanceRangeDTOValidator() 
        {
            RuleFor(x => x.CourseScheduleId)
                .NotEmpty().WithMessage("Dərs cədvəli seçilməlidir.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Tarix qeyd olunmalıdır.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Gələcək tarix üçün davamiyyət qeyd edilə bilməz.");

            RuleForEach(x => x.Students)
                .SetValidator(new UpdateStudentAttendanceDTOValidator());
        }
    }
}
