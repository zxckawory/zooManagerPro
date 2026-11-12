using System;
using System.Collections.Generic;

namespace zooManagerPro.Models;

public partial class AnimalsFeeding
{
    public int Id { get; set; }

    public int AnimalId { get; set; }

    public DateTime FeedingDateTime { get; set; }

    public int TypeOfFeedingId { get; set; }

    public decimal FeedAmount { get; set; }

    public int UnitOfMassId { get; set; }

    public int EmployeeId { get; set; }

    public string? FeedingDescription { get; set; }

    public virtual Animal Animal { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual TypeOfFeeding TypeOfFeeding { get; set; } = null!;

    public virtual UnitOfMass UnitOfMass { get; set; } = null!;

    public string AmountAndUnit
    {
        get
        {
            return $"{FeedAmount}  {UnitOfMass.UnitOfMassName}";
        }
    }
}
