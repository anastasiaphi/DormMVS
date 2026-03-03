using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Student : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кафедра")]
    public int DepartmentId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ступінь")]
    public int DegreeId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ім'я")]

    public string FirstName { get; set; } = null!;
    
    [Display(Name = "По-батькові")]

    public string? MiddleName { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Прізвище")]

    public string LastName { get; set; } = null!;
    [Display(Name = "Номер телефону")]

    public string? PhoneNumber { get; set; }
    [Display(Name = "Ступінь")]
    public virtual Degree? Degree { get; set; } = null!;
    [Display(Name = "Кафедра")]
    public virtual Department? Department { get; set; } = null!;

    public virtual ICollection<RoomAssignment> RoomAssignments { get; set; } = new List<RoomAssignment>();
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
}
