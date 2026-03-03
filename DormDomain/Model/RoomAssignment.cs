using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class RoomAssignment : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кімната")]
    public int RoomId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Студент")]
    public int StudentId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Заселенння")]

    public DateOnly CheckInDate { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Виселенння")]

    public DateOnly? CheckOutDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кімната")]

    public virtual Room? Room { get; set; } = null!;

    public virtual Student? Student { get; set; } = null!;
}
