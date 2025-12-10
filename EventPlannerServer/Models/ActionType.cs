using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class ActionType
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
