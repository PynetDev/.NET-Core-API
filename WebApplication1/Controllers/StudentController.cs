using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    //[Route("api/[controller]")] //default routing 
    //[Route("api/customized")] //customized routing 
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All",Name = "GetStudentDetails")]
        public ActionResult GetStudentDetails()
        {
            //200 - Success
            var students = StudentRepository.Students.Select(i=>new StudentDTO
            {
                id =i.id,
                name =i.name,
                email=i.email,
                address= i.address
        
            });
            return Ok(students);
        }
        
        [HttpGet]
        [Route("{id}", Name = "GetStudentDetailsById")]
        [ProducesResponseType(400)] //Documenting status codes so that front end code can able to capture this responses more efficiently
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult GetStudentDetailsById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Input Id");
            try
            {
                var result = StudentRepository.Students.FirstOrDefault(i => i.id == id);
                if (result == null)
                    return NotFound($"The student with id:{id} not found");
                StudentDTO student = new StudentDTO {
                    address = result.address,
                    name = result.name,
                    id=result.id,
                    email=result.email
                };
                return Ok(student);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error: ");
                
            }
        }

        [HttpGet("{name:alpha}",Name = "GetStudentDetailsByName")]
        [ProducesResponseType(200,Type=typeof(Student))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult GetStudentDetailsByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest("Name cannot be null or empty");
            try
            {
                var res= StudentRepository.Students.FirstOrDefault(i => i.name == name);
                if (res == null)
                    return NotFound($"The student with name:{name} not found");
                var StudentDTO  = new
                {
                    address = res.address,
                    name = res.name,
                    id = res.id,
                    email = res.email
                };
                return Ok(StudentDTO);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error: ");
             
            }
            
        }

        [HttpDelete("{id:min(1):max(100)}",Name = "DeleteStudentDetailsById")] // we can restrict the users input by using various parameters available e.g. min, max etc...
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult DeleteStudentDetailsById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Input Id");
            try
            {
                var stdobj = StudentRepository.Students.FirstOrDefault(i => i.id == id);
                if (stdobj != null)
                {
                    StudentRepository.Students.Remove(stdobj);
                    return Ok(true);
                }
                else return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult AddStudentDetails([FromBody]List<StudentDTO> studentslst) {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);   // this code is only required when we remove [ApiController] attribute it will do manual validation on properties of an entity class
            try
            {
                if(studentslst.Count == 0 || studentslst == null)
                    return BadRequest("Invalid Request");
                int newId = StudentRepository.Students.LastOrDefault()?.id + 1 ?? 1;

                var studentsToAdd = studentslst.Select(student => new Student
                {
                    id = newId++, // post-increment ensures each student gets a unique ID
                    name = student.name,
                    email = student.email,
                    address = student.address
                }).ToList();

                StudentRepository.Students.AddRange(studentsToAdd);
                return CreatedAtRoute("GetStudentDetails",true);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }
             
        }
    }
}
