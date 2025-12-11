using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerLibrary.ResponseDTOs
{
    public class UserResponse
    {
        [Required]
        [StringLength(20, MinimumLength = 3 )]
        public string Login { get; set; }
    }
}
