using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string ProductName { get; set; } = null!;

    public double Price { get; set; }

    public int Quantity { get; set; }

    public double? Discounts { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int Status { get; set; }

    public int CategoryId { get; set; }

    public int ImageId { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Image Image { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    //public double TotalPrice { get; set; }
}
