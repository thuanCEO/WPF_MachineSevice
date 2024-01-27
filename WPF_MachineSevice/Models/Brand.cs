using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string NameLogo { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int UserId { get; set; }

    public int Status { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public DateTime? DeletionDate { get; set; }

    public string? DeleteBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<ShopStore> ShopStores { get; set; } = new List<ShopStore>();

    public virtual User User { get; set; } = null!;
}
