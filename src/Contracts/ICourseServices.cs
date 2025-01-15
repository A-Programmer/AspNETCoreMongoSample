using SchoolApi.Models;

namespace SchoolApi.Contracts;

public interface ICourseServices
{
    Task<Course?> Create(Course course);
    Task<Course?> GetById(string id);
}