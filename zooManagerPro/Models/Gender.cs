using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class Gender
{
    public int Id { get; set; }

    public string GenderName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public override string ToString()
    {
        return GenderName;
    }
}
