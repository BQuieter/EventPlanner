using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventPlannerClient.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string OwnerLogin { get; set; }
        public string Description { get; set; }
        public byte Importance {  get; set; }
        public DateTime DateTime { get; set; }
    }
}
