namespace SchoolApi.Models;

public class Student 
{ 
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public string? Id { get; set; } 

    [Required(ErrorMessage = "First name is required")] 
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")] 
    public string LastName { get; set; } = null!;
    
    public string Major { get; set; } = null!;
}