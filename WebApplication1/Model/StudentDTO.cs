using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebApplication1.CustomValidators;

namespace WebApplication1.Model
{
    public class StudentDTO
    {
        [ValidateNever]
        public int id { get; set; }

        [Required(ErrorMessage ="student name required")]
        [StringLength(50,ErrorMessage ="Maximum 50 characters allowed for student name")]
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string email { get; set; }

        [Required(ErrorMessage = "student address required")]
        public string address { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage ="Password doesn't match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(3,18)]
        public int Age { get; set; }

        [AdmissionDate] //Custom attribute from custom validators folder
        public DateTime AdmissionDate { get; set; }
    }
}
