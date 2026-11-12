using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class TypeOfEnclosure
{
    public int Id { get; set; }

    public string TypeOfEnclosureName { get; set; } = null!;

    public virtual ICollection<Enclosure> Enclosures { get; set; } = new List<Enclosure>();

    public override string ToString()
    {
        return TypeOfEnclosureName;
    }
}
