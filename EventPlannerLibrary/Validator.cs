using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerLibrary
{
    public class Validator<T>
    {
        public static bool IsValid(T Object)
        {
            var context = new ValidationContext(Object);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(Object, context, results);
        }
    }
}
