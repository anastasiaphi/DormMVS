using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Faculty : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Назва факультету")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Адреса")]
    public string? Address { get; set; }

    [Display(Name = "Пошта")]
    public string? Email { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
}
