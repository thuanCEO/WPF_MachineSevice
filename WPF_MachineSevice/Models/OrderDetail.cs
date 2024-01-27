using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public int Status { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Product Product { get; set; } = null!;
}
