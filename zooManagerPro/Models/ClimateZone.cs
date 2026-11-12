using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class ClimateZone
{
    public int Id { get; set; }

    public string ClimateZoneName { get; set; } = null!;

    public virtual ICollection<Enclosure> Enclosures { get; set; } = new List<Enclosure>();
}
