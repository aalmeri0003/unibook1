using System.ComponentModel.DataAnnotations;

namespace UniBook.Models
{
    // A book in UniBook (one row in the Book table)
    public class Book
    {
        public int id { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [Display(Name = "Course Code")]
        [StringLength(10)]
        public string courseCode { get; set; } = "";

        [Required(ErrorMessage = "Book name is required")]
        [Display(Name = "Book Name")]
        [StringLength(200)]
        public string title { get; set; } = "";

        [Required(ErrorMessage = "Choose a faculty")]
        [Display(Name = "Faculty")]
        public string faculty { get; set; } = "";

        [Required(ErrorMessage = "Version is required")]
        [Display(Name = "Version")]
        [StringLength(5)]
        public string version { get; set; } = "";

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        [Display(Name = "New Book Price (SR)")]
        public int newPrice { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10000")]
        [Display(Name = "Used Book Price (SR)")]
        public int usedPrice { get; set; }
    }
}
