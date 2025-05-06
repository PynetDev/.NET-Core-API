using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class CollegeDBContext :DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
        }
        DbSet<Student> Students { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           //Applying Students Table Configuration
           modelBuilder.ApplyConfiguration(new Config.StudentConfig());


        }
    }


}
