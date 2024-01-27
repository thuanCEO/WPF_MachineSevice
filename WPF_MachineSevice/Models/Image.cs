using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class Image
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int Status { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
