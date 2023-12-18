using System;
using System.Collections.Generic;

namespace FitnisTracker.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? Username { get; set; }

    public string? Email { get; set; }

    public double? StartingWeight { get; set; }

    public double? CurrentWeight { get; set; }

    public double? DesiredWeight { get; set; }

    public double? HeightIn { get; set; }

    public string? Gender { get; set; }

    public byte[]? Birthday { get; set; }

    public long? Age { get; set; }

    public long? CalorieLimit { get; set; }

    public string? Activity { get; set; }

    public virtual ICollection<CaloryLog> CaloryLogs { get; set; } = new List<CaloryLog>();

    public virtual ICollection<WeightLog> WeightLogs { get; set; } = new List<WeightLog>();
}
