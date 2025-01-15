using Microsoft.AspNetCore.Mvc;
using SchoolApi.Constants;
using SchoolApi.Contracts;
using SchoolApi.Models;

namespace SchoolApi.Controllers;

[Route(Routes.Root)]
[ApiController]
public sealed class CoursesController : ControllerBase
{
    private readonly ICourseServices _courseServices;

    public CoursesController(ICourseServices courseServices)
    {
        _courseServices = courseServices;
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Course>> Get(string id)
    {
        var course = await _courseServices.GetById(id);
        return course is null ? NotFound() : Ok(course);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Course course)
    {
        var createdCourse = await _courseServices.Create(course);
        
        return createdCourse is null
            ? throw new Exception("Course creation failed")
            : CreatedAtAction(nameof(Get),
                new
                {
                    id = createdCourse.Id
                }, createdCourse);
    }
}