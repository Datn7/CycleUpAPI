using CycleUpAPI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CycleUpAPI.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator(CycleContext cycleContext)
        {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Password).Equal(x => x.ConfirmPassword);
            RuleFor(x => x.Email).Custom((value, context) =>
              {
                  var userAlreadyExists = cycleContext.Users.Any(user => user.Email == value);

                  if (userAlreadyExists)
                  {
                      context.AddFailure("Email", "That Email is taken");
                  }
              });
        }
    }
}
