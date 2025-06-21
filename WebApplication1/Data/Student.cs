using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using WebApplication1.CustomValidators;

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

        [AdmissionDate] //Custom attribute from custom validators folder
        public DateTime AdmissionDate { get; set; }

    }
}
