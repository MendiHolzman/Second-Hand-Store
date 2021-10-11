using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspProject.Models
{
    public class User
    {
        public User()
        {
            Birthday = DateTime.Today;
            products = new List<Product>();
        }
        

        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "חובה להזין שם פרטי")]
        [RegularExpression("^[a-zA-Z א-ת]*$", ErrorMessage = "אנא הזן אותיות בלבד בשדה שם פרטי")]
        [MaxLength(50, ErrorMessage = "שם פרטי עד 50 תוים")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "חובה להזין שם משפחה")]
        [RegularExpression("^[a-zA-Z א-ת ]*$", ErrorMessage = "אנא הזן אותיות בלבד בשדה שם משפחה")]
        [MaxLength(50, ErrorMessage = "שם משפחה עד 50 תוים")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "חובה להזין תאריך לידה")]
        [DataType(DataType.Date, ErrorMessage ="חובה להזין תאריך תקף")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "חובה להזין כתובת מייל")]
        [MaxLength(50, ErrorMessage = "כתובת מייל עד 50 תוים")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "חובה להזין שם משתמש")]
        [MaxLength(50, ErrorMessage = "שם משתמש עד 50 תוים")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "חובה להזין סיסמא")]
        [DataType(DataType.Password)]
        [MaxLength(50, ErrorMessage = "סיסמא עד 50 תוים")]
        public string Password { get; set; }
        [Required(ErrorMessage = "חובה להזין אימות סיסמא")]
        [Compare("Password", ErrorMessage = ".הסיסמאות אינם תואמות, נסה שוב")]
        public string ConfirmPassword { get; set; }
       
        public List<Product> products { get; set; }   
    }
}
