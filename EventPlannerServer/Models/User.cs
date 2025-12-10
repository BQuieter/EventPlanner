using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class User
{
    public string Login { get; set; } = null!;

    public byte Role { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual Role RoleNavigation { get; set; } = null!;
}
