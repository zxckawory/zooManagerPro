using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class UnitOfMass
{
    public int Id { get; set; }

    public string UnitOfMassName { get; set; } = null!;

    public virtual ICollection<AnimalsFeeding> AnimalsFeedings { get; set; } = new List<AnimalsFeeding>();
}
