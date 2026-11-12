using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class Species
{
    public int Id { get; set; }

    public string SpeciesName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public override string ToString()
    {
        return SpeciesName;
    }
}
