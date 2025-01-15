using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolApi.Contracts;
using SchoolApi.Models;

namespace SchoolApi.Services;

public class StudentServices : IStudentServices
{
    private readonly IMongoCollection<Student> _studentCollection;

    public StudentServices(IOptions<SchoolDatabaseSettings> schoolDatabaseSettings,
        IMongoClient client)
    {
        IMongoDatabase database = client
            .GetDatabase(schoolDatabaseSettings.Value.DatabaseName);

        _studentCollection = database
            .GetCollection<Student>(
                schoolDatabaseSettings
                    .Value
                    .StudentsCollectionName);
    }
    
    public async Task<Student?> Create(Student student)
    {
        student.Id = ObjectId.GenerateNewId().ToString();

        await _studentCollection.InsertOneAsync(student);
        return student;
    }

    public async Task<DeleteResult> Delete(string id)
    {
        return await _studentCollection.DeleteOneAsync(s => s.Id == id);
    }

    public async Task<List<Student>> GetAll()
    {
        return await _studentCollection.Find(s => true).ToListAsync();
    }

    public async Task<Student?> GetById(string id)
    {
        return await _studentCollection.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<ReplaceOneResult> Update(string id, Student student)
    {
        return await _studentCollection.ReplaceOneAsync(s => s.Id == id, student);
    }
}