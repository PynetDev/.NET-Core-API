using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Model;

namespace WebApplication1.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddStudentDetailsAsync(List<Student> studentslst)
        {
            if (studentslst == null)
                throw new ArgumentNullException("students list should n't be null");

            _dbContext.Students.AddRange(studentslst);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteStudentDetailsByIdAsync(int id)
        {
            var stdobj = await _dbContext.Students.FirstOrDefaultAsync(i => i.id == id);
            if (stdobj == null)
                throw new ArgumentNullException("student id is not found");
            _dbContext.Students.Remove(stdobj);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetStudentDetailsAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetStudentDetailsByIdAsync(int id)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(i => i.id == id);
        }

        public async Task<Student> GetStudentDetailsByNameAsync(string name)
        {
            
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Student name must not be null or empty.", nameof(name));

            return await _dbContext.Students.FirstOrDefaultAsync(s => EF.Functions.Like(s.name, name));
        }

        //public async Task<bool> UpdatePasswordAsync(int id, JsonPatchDocument<Student> patchDocument)
        //{
        //    if (patchDocument == null || id < 0)
        //        throw new Exception("Bad Request");

        //    var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.id == id);
        //    if (student == null)
        //        throw new ArgumentNullException($"student details not found for id: {id}");
        //    patchDocument.ApplyTo(student);

        //    if (string.IsNullOrWhiteSpace(student.password))
        //        throw new ArgumentNullException("password should n't be null");

        //    await _dbContext.SaveChangesAsync();
        //    return true;
        //}

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            if (student == null || student.id <= 0)
                throw new Exception("Given student details either null or student id should be vaid id");
            var stdobj = await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(i => i.id == student.id);
            if (stdobj == null)
                throw new ArgumentNullException("Student not found with given std id");

            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
            return _dbContext.Students.FirstOrDefault(i => i.id == student.id);
        }
    }
}
