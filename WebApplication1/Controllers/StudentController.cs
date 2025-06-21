using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Data;
using WebApplication1.ExceptionHandler;
using WebApplication1.Logging;
using WebApplication1.Model;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    //[Route("api/[controller]")] //default routing 
    //[Route("api/customized")] //customized routing 
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        //private readonly IMyLogger _ilogger;
        private readonly ILogger<StudentController> _ilogger;
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        public StudentController(ILogger<StudentController> ilogger, IMapper mapper, IStudentRepository repository, IMemoryCache cache)
        {
            _ilogger = ilogger;
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
        }
        [HttpGet]
        [Route("All", Name = "GetStudentDetails")]
        public async Task<ActionResult> GetStudentDetailsAsync()
        {
            _ilogger.LogInformation("GetStudentDetails method starts");
            //200 - Success
            var students = await _repository.GetStudentDetailsAsync();
            var result = _mapper.Map<List<StudentDTO>>(students);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}", Name = "GetStudentDetailsById")]
        [ProducesResponseType(400)] //Documenting status codes so that front end code can able to capture this responses more efficiently
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetStudentDetailsByIdAsync(int id)
        {
            string cacheKey = $"student_{id}";
            _ilogger.LogInformation("GetStudentDetailsById method starts");

            if (id <= 0)
            {
                _ilogger.LogWarning("Invalid student ID: {Id}", id);
                return BadRequest("Invalid Input Id");
            }

            if (!_cache.TryGetValue(cacheKey, out Student studentRecord))
            {
                var sw = Stopwatch.StartNew();

                studentRecord = await _repository.GetStudentDetailsByIdAsync(id);
                sw.Stop();

                if (studentRecord == null)
                {
                    _ilogger.LogWarning("Student not found in DB for ID: {Id}", id);
                }
                else
                {
                    _ilogger.LogInformation("Cache MISS - DB call took {Time} ms", sw.ElapsedMilliseconds);

                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                    _cache.Set(cacheKey, studentRecord, cacheOptions);
                }
            }
            else
            {
                _ilogger.LogInformation("Cache HIT for student ID: {Id}", id);
            }

            if (studentRecord == null)
            {
                _ilogger.LogError("Student with ID {Id} not found", id);
                throw new KeyNotFoundException($"The student with id:{id} not found");
            }

            var studentDto = _mapper.Map<StudentDTO>(studentRecord);
            return Ok(studentDto);
        }


        [HttpGet("{name:alpha}", Name = "GetStudentDetailsByName")]
        [ProducesResponseType(200, Type = typeof(Student))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetStudentDetailsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name cannot be null or empty");

            var res = await _repository.GetStudentDetailsByNameAsync(name);

            var acceptHeader = HttpContext.Request.Headers["Accept"].ToString();
            if (!string.IsNullOrEmpty(acceptHeader) && !acceptHeader.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                throw new NotAcceptableException("Only 'application/json' responses are supported.");
            }

            if (res == null)
                return NotFound($"The student with name '{name}' was not found.");

            var studentDTO = _mapper.Map<StudentDTO>(res);
            return Ok(studentDTO);
        }

        [HttpDelete("{id:min(1):max(100)}", Name = "DeleteStudentDetailsById")] // we can restrict the users input by using various parameters available e.g. min, max etc...
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteStudentDetailsByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Input Id");
            try
            {
                var stdobj = await _repository.GetStudentDetailsByIdAsync(id);
                return NotFound();
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
        public async Task<ActionResult> AddStudentDetailsAsync([FromBody] List<StudentDTO> studentslst)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);   // this code is only required when we remove [ApiController] attribute it will do manual validation on properties of an entity class

            try
            {
                if (studentslst.Count == 0 || studentslst == null)
                    return BadRequest("Invalid Request");
                //int newId = _dbContext.Students.OrderBy(i=>i).LastOrDefault()?.id + 1 ?? 1;
                var studentsToAdd = _mapper.Map<List<Student>>(studentslst);
                await _repository.AddStudentDetailsAsync(studentsToAdd);
                return CreatedAtRoute("GetStudentDetails", true);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal Server Error");
            }

        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO student)
        {
            if (student == null || student.id <= 0)
                return BadRequest();
            var newRecord = _mapper.Map<Student>(student);
            await _repository.UpdateStudentAsync(newRecord);
            return NoContent(); // will give no content but successfully update student details 
        }

        //[HttpPatch]
        //[Route("{id:int}/UpdatePassword")]
        //[ProducesResponseType(400)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(500)]
        //public async Task<ActionResult> UpdatePasswordAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        //{
        //    if (patchDocument == null || id < 0)
        //        return BadRequest();

        //    _mapper.Map<JsonPatchDocument<Student>>(patchDocument);
        //    await _repository.UpdatePasswordAsync(id, patchDocument);
        //    return Ok();

        //}

        //Dependency Injection Example
        [HttpGet]
        public ActionResult DIExample()
        {
            _ilogger.LogTrace("LogTrace");
            _ilogger.LogDebug("LogDebug");
            _ilogger.LogInformation("LogInformation");
            _ilogger.LogWarning("LogWarning");
            _ilogger.LogError("LogError");
            _ilogger.LogCritical("LogCritical");
            return Ok();
        }

        // Global Exception Handling
        [HttpGet("throw")]
        public IActionResult ThrowException([FromQuery] string type)
        {
            switch (type?.ToLower())
            {
                case "argument":
                    throw new ArgumentException("This is a bad request (argument exception).");

                case "key":
                    throw new KeyNotFoundException("This key was not found.");

                case "unauthorized":
                    throw new UnauthorizedAccessException("You are not authorized.");



                default:
                    throw new Exception("This is a general unhandled exception.");
            }
        }

        [HttpGet("PagedResponse")]
        public ActionResult GetPagedResponse(int page = 1, int size = 10)
        {
            var items = Enumerable.Range(1, 1000)
                .Select(i => new
                {
                    itemId = i,
                    itemName = $"Item {i}"
                });

            var result=items.Skip((page - 1) * size).Take(size).ToList();
            return Ok(result);
        }
    }
}
