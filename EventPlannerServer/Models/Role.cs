using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class Role
{
    public byte Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
