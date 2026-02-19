using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Department : Entity
{
   
    public int FacultyId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Faculty Faculty { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
