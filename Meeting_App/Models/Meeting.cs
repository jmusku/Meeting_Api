using System;
using FluentValidation.Attributes;
using Meeting_App.Validation;

namespace Meeting_App.Models
{
    [Validator(typeof(MeetingValidator))]
    public class Meeting
    {
        public int Id { get; set; }
        public int CreatedById { get; set; }
        public string Subject { get; set; }
        public string Agenda { get; set; }
        public DateTime Date { get; set; }
        public string Attendees { get; set; }
    }
}