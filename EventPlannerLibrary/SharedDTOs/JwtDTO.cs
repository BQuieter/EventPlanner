using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerLibrary.SharedDTOs
{
    public class JwtDTO
    {
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }
        [Required]
        public string JWT { get; set; }
        public string? Refresh {  get; set; }
    }
}
