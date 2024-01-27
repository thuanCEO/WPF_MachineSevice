using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class OrderImage
{
    public int Id { get; set; }

    public int ImageDetailsId { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
