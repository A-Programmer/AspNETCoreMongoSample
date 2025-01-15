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
    public StudentsController(IStudentServices studentServices)
    {
        _studentServices = studentServices;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> Get()
        => (await _studentServices.GetAll());

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Student>> GetById(string id)
    {
        Student student = await _studentServices.GetById(id);

        return student is null ? NotFound() : Ok(student);
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