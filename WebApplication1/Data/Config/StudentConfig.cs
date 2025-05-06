using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace WebApplication1.Data.Config
{
    public class StudentConfig:IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.id);
            builder.Property(s=>s.id).UseIdentityColumn();
            builder.Property(s => s.name)
                .IsRequired()
                .HasMaxLength(250);
            builder.Property(s => s.email)
            .IsRequired()
            .HasMaxLength(250);
            builder.Property(s => s.address).IsRequired(false).HasMaxLength(500);
            builder.Property(s => s.password).IsRequired().HasMaxLength(30);
            builder.Property(s => s.dateOfBirth).IsRequired();


            builder.HasData(new List<Student>
           {
               new Student
                {
                    id = 1,
                    name = "John",
                    email = "john@gmail.com",
                    address = "Miami, USA",
                    password="john@123",
                    dateOfBirth= new DateOnly(2017,6,17),
                },
                new Student
                {
                    id = 2,
                    name = "smith",
                    email = "smith@gmail.com",
                    address = "Texas, USA",
                    password="smith@123",
                    dateOfBirth= new DateOnly(2014, 3, 29),
                }

           });
        }
    }
  
}
