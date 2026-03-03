using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Degree : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ступінь освіти")]
    public string DegreeName { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
