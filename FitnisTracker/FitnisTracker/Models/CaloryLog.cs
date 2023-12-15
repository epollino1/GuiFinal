using System;
using System.Collections.Generic;

namespace FitnisTracker.Models;

public partial class CaloryLog
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public byte[]? LoggedAt { get; set; }

    public string? Title { get; set; }

    public long? Calories { get; set; }

    public virtual User? User { get; set; }
}
