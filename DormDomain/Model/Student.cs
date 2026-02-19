using System;
using System.Collections.Generic;

namespace DormDomain.Model;

public partial class Student : Entity
{

    public int DepartmentId { get; set; }

    public int DegreeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public virtual Degree Degree { get; set; } = null!;

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<RoomAssignment> RoomAssignments { get; set; } = new List<RoomAssignment>();
}
