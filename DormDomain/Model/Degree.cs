using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Degree : Entity
{

    public string DegreeName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
