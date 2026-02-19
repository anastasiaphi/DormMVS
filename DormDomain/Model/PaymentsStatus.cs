using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class PaymentsStatus : Entity
{

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
