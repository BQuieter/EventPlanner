using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EventPlannerLibrary.RequestDTOs
{
    public class AuthorizationUserRequest
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login { get; set; }

        [Required]
        [StringLength (20, MinimumLength = 3)] 
        public string Password { get; set; }
    }
}
