using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DormDomain.Model;

public partial class Tariff : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name= "Назва тарифу")]
    public string TariffsName { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ціна за місяць")]
    public decimal PricePerMonth { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
