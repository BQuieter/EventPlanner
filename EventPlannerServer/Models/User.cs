using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class User
{
    public string Login { get; set; } = null!;

    public byte RoleId { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual Role Role { get; set; } = null!;
}
