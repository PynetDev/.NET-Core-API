using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Model
{
    public static class StudentRepository
    {

        public static  List<Student> Students { get; set; } = new List<Student>{
                new Student
                {
                    id = 1,
                    name = "John",
                    email = "john@gmail.com",
                    address = "Miami, USA"
                },
                new Student
                {
                    id = 1,
                    name = "doe",
                    email = "doe@gmail.com",
                    address = "Texas, USA"
                }
            };
    }
}
