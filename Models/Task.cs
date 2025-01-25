using System.ComponentModel.DataAnnotations;

public class Task
{
    public int TaskId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [MaxLength(150, ErrorMessage = "Title cannot exceed 150 characters")]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public string Status { get; set; } = "Pending";

    [Required(ErrorMessage = "AssignedTo is required")]
    public int AssignedTo { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;
}