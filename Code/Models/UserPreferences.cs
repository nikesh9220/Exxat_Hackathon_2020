using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Models
{
    public class UserPreferences
    {
        [Key]
        public int PreferenceID { get; set; }
        public int UserID { get; set; }
        public string PreferanceName { get; set; }
        public string PreferanceValue { get; set; }
    }
}
