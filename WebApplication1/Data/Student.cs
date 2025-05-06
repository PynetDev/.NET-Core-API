using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace WebApplication1.Data
{
    public class Student
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public DateOnly dateOfBirth { get; set; }


    }
}
