using MongoDB.Driver;
using SchoolApi.Models;

namespace SchoolApi.Contracts;

public interface IStudentServices
{
    Task<Student?> Create(Student student);
    Task<DeleteResult> Delete(string id);
    Task<List<Student>> GetAll();
    Task<Student?> GetById(string id);
    Task<ReplaceOneResult> Update(string id, Student student);
}