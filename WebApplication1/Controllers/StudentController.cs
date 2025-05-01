using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Student> GetStudentDetails()
        {
            return StudentRepository.Students;
        }
        [HttpGet("{id:int}")]
        public Student GetStudentDetailsById(int id)
        {
            return StudentRepository.Students.FirstOrDefault(i => i.id == id);
        }
    }
}
