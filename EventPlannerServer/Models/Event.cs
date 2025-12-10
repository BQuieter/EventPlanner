using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Owner { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public byte Importance { get; set; }

    public string Description { get; set; } = null!;

    public virtual EventImportance ImportanceNavigation { get; set; } = null!;

    public virtual User OwnerNavigation { get; set; } = null!;
}
