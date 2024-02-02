using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public int StoreId { get; set; }

    public int OrderDetailsId { get; set; }

    public int OrderImageId { get; set; }

    public int Status { get; set; }

    public double TotalPrice { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual Machine Machine { get; set; } = null!;

    public virtual OrderDetail OrderDetails { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetailsNavigation { get; set; } = new List<OrderDetail>();

    public virtual OrderImage OrderImage { get; set; } = null!;

    public virtual ShopStore Store { get; set; } = null!;
}
