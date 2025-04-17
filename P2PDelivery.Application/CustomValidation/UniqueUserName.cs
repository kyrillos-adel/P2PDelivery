using Microsoft.AspNetCore.Identity;
using P2PDelivery.Domain.Entities;
using System.ComponentModel.DataAnnotations;


namespace P2PDelivery.Application.CustomValidation;

public class UniqueUserName : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var userManager = (UserManager<User>)validationContext.GetService(typeof(UserManager<User>));
        var userName = value as string;

        var user = userManager.FindByNameAsync(userName).Result; 

        if (user != null)
        {
            return new ValidationResult("Username already exists.");
        }

        return ValidationResult.Success;
    }
}
