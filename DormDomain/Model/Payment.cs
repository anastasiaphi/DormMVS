using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Payment : Entity
{

    public int RoomAssignmentId { get; set; }

    public int PaymentsStatusId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public virtual PaymentsStatus PaymentsStatus { get; set; } = null!;

    public virtual RoomAssignment RoomAssignment { get; set; } = null!;
}
