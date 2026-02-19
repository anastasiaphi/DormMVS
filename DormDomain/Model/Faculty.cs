using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Faculty : Entity
{

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
