
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using Lab5.Application.ViewModels;
using Lab5.Domain.Models;

namespace Lab5.Application.Validators;
public class StudentValidator : AbstractValidator<StudentDto>
{
    public StudentValidator()
    {
        /*RuleFor(s => s.Id).NotNull().GreaterThan(0)
            .WithMessage("StudentId must be not null and greater than zero");*/
        
        RuleFor(s=>s.StudentName).NotNull().NotEmpty()
            .WithMessage("Name must be not null and not empty");
        
        RuleFor(s => s.Email).NotEmpty()
            .EmailAddress().WithMessage("A valid email is required.");
        
    }
}