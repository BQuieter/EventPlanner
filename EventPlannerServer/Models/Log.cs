using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventPlannerServer.Models;

public partial class Log
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public DateTime DateTime { get; set; }

    public byte TypeId { get; set; }

    public string? Description { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual ActionType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
