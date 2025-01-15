using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SchoolApi.Constants;
using SchoolApi.Contracts;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[Route(Routes.Root)]
[ApiController]
public sealed class StudentsController : ControllerBase
{
    private readonly IStudentServices _studentServices;
    private readonly ICourseServices _courseServices;
    public StudentsController(IStudentServices studentServices,
        ICourseServices courseServices)
    {
        _studentServices = studentServices;
        _courseServices = courseServices;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Get()
        => (await _studentServices.GetAll());

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Student>> GetById(string id)
    {
        var student = await _studentServices.GetById(id);

        if (student is null)
        {
            return NotFound();
        }
        if (student.Courses is null || !student.Courses.Any())
        {
            return Ok(student);
        }

        student.CourseList ??= new();
        foreach (string courseId in student.Courses)
        {
            Course course = await _courseServices.GetById(courseId) ?? throw new Exception("Invalid Course Id");
            
            student.CourseList.Add(course);
        }

        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Student student)
    {
        Student createdStudent = await _studentServices.Create(student);

        return createdStudent is null
            ? throw new Exception("Student creation failed.")
            : CreatedAtAction(nameof(GetById), new
            {
                id = createdStudent.Id
            }, createdStudent);
    }
    
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Student updatedStudent)
    {
        var queriedStudent = await _studentServices.GetById(id);
            
        if (queriedStudent is null)
        {
            return NotFound();
        }
        await _studentServices.Update(id, updatedStudent);
            
        return NoContent();
    }
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var student = await _studentServices.GetById(id);
        if (student is null)
        {
            return NotFound();
        }
        await _studentServices.Delete(id);
        return NoContent();
    }
}