using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class University
    {
        [Key]
        public int UniversityID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DisplayFormat(NullDisplayText = "No addresses")]
        public string Adress { get; set; }

       public Department Department { get; set; }
    }
}