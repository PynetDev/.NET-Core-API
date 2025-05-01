using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class StudentDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage ="student name required")]
        public string name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "student address required")]
        public string address { get; set; }
    }
}
