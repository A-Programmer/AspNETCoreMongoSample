﻿using SchoolApi.Contracts;

namespace SchoolApi.Models;

[BsonIgnoreExtraElements]
public class Student  : IEntity
{ 
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)] 
    public string? Id { get; set; } 

    [Required(ErrorMessage = "First name is required")] 
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required")] 
    public string LastName { get; set; } = null!;
    
    public string Major { get; set; } = null!;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public List<string> Courses { get; set; } = null!;

    [BsonIgnore]
    public List<Course> CourseList { get; set; } = null!;
}