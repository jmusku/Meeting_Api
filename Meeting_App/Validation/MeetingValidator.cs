using FluentValidation;
using Meeting_App.Models;
using Meeting_App.Services;

namespace Meeting_App.Validation
{
    public class MeetingValidator : AbstractValidator<Meeting>
    {
        public MeetingValidator()
        {
            //RuleFor(x => x.CreatedById).NotEmpty().WithMessage("The Created By Id cannot be blank.");

            RuleFor(x => x.Agenda).NotEmpty().WithMessage("The meeting Agenda cannot be blank.")
                .Length(0, 100).WithMessage("The Meeting Agenda cannot be more than 100 characters.");

            RuleFor(x => x.Subject).NotEmpty().WithMessage("The meeting subject cannot be blank.")
                .Length(0, 50).WithMessage("The Meeting Subject cannot be more than 50 characters.");

            RuleFor(x => x.Date).NotEmpty().WithMessage("The meeting date cannot be blank.");

            //RuleFor(x => x.Attendees).NotEmpty().WithMessage("The meeting attendees cannot be blank.")
            //    .Length(0, 50).WithMessage("At least one meeting attendee should be there and can not be more than 10.")
            //    .Must(attendees => Utility.CheckAttendeeLength(attendees)).WithMessage("A valid meeting date is require")
            //    .Must(attendee => Utility.IsAttendeeHaveValidData(attendee))
            //    .WithMessage("Attendee should have valid data type, a number type");

            RuleFor(x => x.Date).NotEmpty().WithMessage("The meeting Agenda cannot be blank.")
                .Must(meetingDate => Utility.BeAValidDate(meetingDate)).WithMessage("A valid meeting date is require");
        }

    }
}