using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class Event
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime DateTime { get; set; }

    public byte ImportanceId { get; set; }

    public string Description { get; set; } = null!;

    public virtual EventImportance Importance { get; set; } = null!;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual User User { get; set; } = null!;
}
