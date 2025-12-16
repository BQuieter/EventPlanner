using System;
using System.Collections.Generic;

namespace EventPlannerServer.Models;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiredAt { get; set; }

    public virtual User User { get; set; } = null!;
}
