using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class EventImportance
{
    public byte Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
