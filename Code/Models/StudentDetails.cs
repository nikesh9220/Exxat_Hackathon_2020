using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Models
{
    public class StudentDetails
    {
        [Key]
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Batch { get; set; }
        public int? DocumentId { get; set; }
        public decimal? ContactNo { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
       // [Required(ErrorMessage = "Course Name is must")]
        //[Required(ErrorMessage = "University Id is must")]
        //public University University { get; set; } // referential integrity
    }
}
