using System.ComponentModel.DataAnnotations;

namespace P2PDelivery.Domain.Validators
{
    public class NotInPastAttribute: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if(value is DateTime dateValue)
            {
                return dateValue >= DateTime.Now;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"{name} must not be in the past.";
        }
    }
}
