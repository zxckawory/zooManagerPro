using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<AnimalsFeeding> AnimalsFeedings { get; set; } = new List<AnimalsFeeding>();
}
