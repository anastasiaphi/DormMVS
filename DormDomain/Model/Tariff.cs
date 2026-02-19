using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Tariff : Entity
{

    public string TariffsName { get; set; } = null!;

    public decimal PricePerMonth { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
