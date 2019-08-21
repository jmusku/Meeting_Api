using FluentValidation;
using Meeting_App.Models;
using Meeting_App.Services;

namespace Meeting_App.Validation
{
    public class AttendeeValidator : AbstractValidator<Attendee>
    {
        public AttendeeValidator()
        {

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("The First Name cannot be blank.")
                .Length(0, 100).WithMessage("The First Name should be of at-least 8 char and cannot be more than 100 characters.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.")
                .Length(0, 100).WithMessage("The Last Name cannot be more than 100 characters.");

        }
    }

    public class UpdateAttendeeValidator : AbstractValidator<UpdateMeetingAttendee>
    {
        public UpdateAttendeeValidator()
        {
            RuleFor(x => x.MeetingId).GreaterThan(0).WithMessage("The meeting id should be grater than 0.");

            RuleFor(x => x.AttendeeIds).NotEmpty().WithMessage("The attendee cannot be blank.")
                .Length(1, 100).WithMessage("The First Name cannot be more than 100 characters.")
                .Must(attendee => Utility.CheckAttendeeLength(attendee))
                .WithMessage("You can add maximum 10 attendee and minium 1 attendee")
                .Must(attendee => Utility.IsAttendeeHaveValidData(attendee))
                .WithMessage("Attendee should have valid data type, a number type");
        }
    }
}
