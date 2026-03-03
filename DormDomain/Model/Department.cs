using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Department : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва факультету")]
    public int FacultyId { get; set; }
  [Required(ErrorMessage = "Поле не повинно бути порожнім")]
   [Display(Name = "Назва кафедри")]
    public string Name { get; set; } = null!;

    [Display(Name = "Факультет")]
    public virtual Faculty? Faculty { get; set; } = null!;

    public virtual ICollection<Student>? Students { get; set; } = new List<Student>();
}
