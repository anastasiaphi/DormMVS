using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Room : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Тариф")]
    public int TariffsId { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Номер кімнати")]
    public int RoomNum { get; set; }
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Кількість місць")]

    public int Capacity { get; set; }

    public virtual ICollection<RoomAssignment> RoomAssignments { get; set; } = new List<RoomAssignment>();

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Тариф")]
    public virtual Tariff Tariffs { get; set; } = null!;
}
