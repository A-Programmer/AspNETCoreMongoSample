using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApi.Contracts;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class CourseServices : ICourseServices
{
    private readonly IMongoCollection<Course> _courseCollection;

    public CourseServices(IOptions<SchoolDatabaseSettings> schoolSettings,
        IMongoClient client)
    {
        IMongoDatabase database = client.GetDatabase(
            schoolSettings.Value.CoursesCollectionName);

        _courseCollection = database.GetCollection<Course>(schoolSettings.Value.CoursesCollectionName);
    }
    public async Task<Course?> Create(Course course)
    {
        course.Id = ObjectId.GenerateNewId().ToString();

        await _courseCollection.InsertOneAsync(course);
        return course;
    }

    public async Task<Course?> GetById(string id)
    {
        return await _courseCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
    }
}