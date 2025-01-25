using System.ComponentModel.DataAnnotations;

public class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    public string Email { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}