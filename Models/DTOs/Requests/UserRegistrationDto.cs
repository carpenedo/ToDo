using System.ComponentModel.DataAnnotations;

public class UserRegistrationDto{
    [Required]
    [EmailAddress]
    public String Email { get; set; }
    [Required]
    public String Password { get; set; }
}