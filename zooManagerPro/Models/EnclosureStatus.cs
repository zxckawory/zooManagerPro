using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class EnclosureStatus
{
    public int Id { get; set; }

    public string EnclosureStatusName { get; set; } = null!;

    public virtual ICollection<Enclosure> Enclosures { get; set; } = new List<Enclosure>();
}
