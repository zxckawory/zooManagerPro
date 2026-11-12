using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class HealthStatus
{
    public int Id { get; set; }

    public string HealthStatusName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public override string ToString()
    {
        return HealthStatusName;
    }

    public string ColorHealthStatus
    {
        get
        {
            if(Id == 1)
            {
                return "Green";
            }
            if (Id == 2)
            {
                return "Yellow";
            }
            else return "Red";
        }
    }
}
