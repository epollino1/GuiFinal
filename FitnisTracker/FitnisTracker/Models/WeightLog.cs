using System;
using System.Collections.Generic;

namespace FitnisTracker.Models;

public partial class WeightLog
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public byte[]? LoggedAt { get; set; }

    public double? CurrentWeight { get; set; }

    public virtual User? User { get; set; }
}
