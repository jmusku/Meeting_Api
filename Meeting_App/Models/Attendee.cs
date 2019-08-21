using FluentValidation.Attributes;
using Meeting_App.Validation;

namespace Meeting_App.Models
{
    [Validator(typeof(AttendeeValidator))]
    public class Attendee
    {
        public int Id { get; set; }
      
        public string FirstName { get; set; }
     
        public string LastName { get; set; }
    }
}