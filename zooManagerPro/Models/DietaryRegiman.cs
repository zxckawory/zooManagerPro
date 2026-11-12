using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class DietaryRegiman
{
    public int Id { get; set; }

    public string DietName { get; set; } = null!;

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public string DietaryRegimenImage
    {
        get
        {
            if (Id == 1)
            {
                return DietName + " 🥩";
            }
            if (Id == 2)
            {
                return DietName + " 🌿";
            }
            else return DietName + " 🥗";
        }
    }
}
