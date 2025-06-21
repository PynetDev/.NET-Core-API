using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentDetailsAsync();
        Task<Student> GetStudentDetailsByIdAsync(int id);
        Task<Student> GetStudentDetailsByNameAsync(string name);
        Task<bool> DeleteStudentDetailsByIdAsync(int id);
        Task<bool> AddStudentDetailsAsync(List<Student> studentslst);
        Task<Student> UpdateStudentAsync(Student student);


    }
}
