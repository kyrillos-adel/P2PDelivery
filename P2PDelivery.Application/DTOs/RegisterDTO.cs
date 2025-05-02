using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace P2PDelivery.Application.DTOs;

public class RegisterDTO
{
    public string NatId { get; set; }

    [StringLength(20, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name must contain only letters and spaces.")]
    public string FullName {  get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number.")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }

    public IFormFile? ProfileImage { get; set; } 
}
