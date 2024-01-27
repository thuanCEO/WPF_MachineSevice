using System;
using System.Collections.Generic;

namespace WPF_MachineSevice.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string? Description { get; set; }

    public int Status { get; set; }

    public string? Code { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public string? ModificationBy { get; set; }

    public string? IsDeleted { get; set; }

    public virtual ICollection<Brand> Brands { get; set; } = new List<Brand>();
}
