using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class Feature
{
    public int Id { get; set; }

    public string FeaturesName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}
