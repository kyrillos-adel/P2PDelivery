
using Microsoft.AspNetCore.Identity;
using P2PDelivery.Application.Response;
using P2PDelivery.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PDelivery.Application.CustomValidation
{
    public class UniqueUserName : ValidationAttribute
    {

        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userManager = (UserManager<User>)validationContext.GetService(typeof(UserManager<User>));
            var userName = value as string;

            var user = userManager.FindByNameAsync(userName).Result; // Note: Blocking call

            if (user != null)
            {
                return new ValidationResult("Username already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
