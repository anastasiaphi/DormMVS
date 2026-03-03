using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Payment : Entity
{

    public int RoomAssignmentId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Статус оплати")]
    public int PaymentsStatusId { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Сума")]
    public decimal Amount { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дата")]
    public DateTime PaymentDate { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Статус")]
    public virtual PaymentsStatus PaymentsStatus { get; set; } = null!;

    public virtual RoomAssignment RoomAssignment { get; set; } = null!;
}
