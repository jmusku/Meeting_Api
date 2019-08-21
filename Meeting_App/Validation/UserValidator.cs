using FluentValidation;
using Meeting_App.Models;
using Meeting_App.Services;

namespace Meeting_App.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            //RuleFor(x => x.Id).NotEmpty().WithMessage("The user id cannot be blank.");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("The First Name cannot be blank.")
                .Length(0, 100).WithMessage("The First Name cannot be more than 100 characters.");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("The Last Name cannot be blank.")
                .Length(0, 100).WithMessage("The last name cannot be more than 100 characters.");

            RuleFor(x => x.UserRole).NotEmpty().WithMessage("The user role cannot be blank.")
                .Length(3, 100).WithMessage("The user role should be at least of length 3 and cannot be more than 100 characters.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("The password name cannot be blank.")
                .Must(password => Utility.PasswordValidation(password))
                .WithMessage("Sorry password didn't satisfy the custom logic");

            RuleFor(x => x.UserName).Length(5, 999).WithMessage("The user name must be at least 5 characters long.");
        }
    }
}