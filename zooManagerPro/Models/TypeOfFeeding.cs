using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class TypeOfFeeding
{
    public int Id { get; set; }

    public string TypeOfFeedingName { get; set; } = null!;

    public virtual ICollection<AnimalsFeeding> AnimalsFeedings { get; set; } = new List<AnimalsFeeding>();
}
