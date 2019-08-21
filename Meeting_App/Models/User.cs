using FluentValidation.Attributes;
using Meeting_App.Validation;

namespace Meeting_App.Models
{
    [Validator(typeof(UserValidator))]
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; } 
    }
}