using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.DTOs.Attendance;

namespace UniLMS.Application.Validators.Attendances
{
    public class UpdateStudentAttendanceDTOValidator : AbstractValidator<UpdateStudentAttendanceDTO>
    {
        public UpdateStudentAttendanceDTOValidator() 
        {
            RuleFor(x => x.AttendanceId)
                .NotEmpty().WithMessage("Id boş ola bilməz");

            RuleFor(x => x.IsPresent)
                .NotNull().WithMessage("Tələbənin davamiyyət statusu (iştirak edib/etmədiyi) mütləq seçilməlidir.");

            RuleFor(x => x.Note)
                .MaximumLength(250).WithMessage("Qeyd 250 simvoldan çox ola bilməz.");

            When(x => !x.IsPresent, () =>
            {
                RuleFor(x => x.Note)
                    .NotEmpty().WithMessage("Tələbə iştirak etmədikdə səbəb/qeyd göstərilməlidir.");
            });
        }
    }
}
