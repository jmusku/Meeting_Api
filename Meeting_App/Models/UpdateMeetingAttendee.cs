using FluentValidation.Attributes;
using Meeting_App.Validation;


namespace Meeting_App.Models
{
    [Validator(typeof(UpdateAttendeeValidator))]
    public class UpdateMeetingAttendee
    {
        public int MeetingId { get; set; }

        public string AttendeeIds { get; set; }
    }
}