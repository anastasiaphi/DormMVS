using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class RoomAssignment : Entity
{

    public int RoomId { get; set; }

    public int StudentId { get; set; }

    public DateOnly CheckInDate { get; set; }

    public DateOnly? CheckOutDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room Room { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
