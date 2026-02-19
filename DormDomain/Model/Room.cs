using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Room : Entity
{
 
    public int TariffsId { get; set; }

    public int RoomNum { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<RoomAssignment> RoomAssignments { get; set; } = new List<RoomAssignment>();

    public virtual Tariff Tariffs { get; set; } = null!;
}
