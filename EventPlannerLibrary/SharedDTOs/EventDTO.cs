using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerLibrary.SharedDTOs
{
    public class EventDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string User { get; set; }
        [Required]
        public string Event { get; set; }
        [Required]
        public byte Importance { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
    }
}
