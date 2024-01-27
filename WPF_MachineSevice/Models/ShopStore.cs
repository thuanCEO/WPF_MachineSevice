using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class ShopStore
{
    public int Id { get; set; }

    public string StoreName { get; set; } = null!;

    public string? Description { get; set; }

    public string Address { get; set; } = null!;

    public int Status { get; set; }

    public int BrandId { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual ICollection<Machine> Machines { get; set; } = new List<Machine>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
