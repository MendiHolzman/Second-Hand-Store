using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyAspProject.Models
{
    public class Product 
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "חובה להזין כותרת")]
        [MaxLength(50, ErrorMessage = "עד 50 תוים")]
        public string Title { get; set; }
        [Required(ErrorMessage = "חובה להזין תיאור קצר")]
        [MaxLength(500, ErrorMessage = "עד 500 תוים")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "חובה להזין תיאור ארוך")]
        [MaxLength(4000, ErrorMessage = "עד 4000 תוים")]
        public string LongDescription { get; set; }
        [Required(ErrorMessage = "חובה להזין מחיר")]
        [DataType(DataType.Currency)]
        [Range(1,18,ErrorMessage ="חובה להזין סכום בין 1 ל18")]
        public int Price { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile FImage1 { get; set; }
        public string Image1 { get; set; }
        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile FImage2 { get; set; }
        public string Image2 { get; set; }
        [DataType(DataType.Upload)]
        [NotMapped]
        public IFormFile FImage3 { get; set; }
        public string Image3 { get; set; }
        [Required]
        public int State { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public bool Selected { get; set; }

    }
}
