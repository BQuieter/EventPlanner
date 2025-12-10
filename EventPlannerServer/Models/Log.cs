using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class Log
{
    public int Id { get; set; }

    public string Owner { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public byte Type { get; set; }

    public string Description { get; set; } = null!;

    public virtual User OwnerNavigation { get; set; } = null!;

    public virtual ActionType TypeNavigation { get; set; } = null!;
}
