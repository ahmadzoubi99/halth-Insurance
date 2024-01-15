using System;
using System.Collections.Generic;

namespace Health_Insurance.Models;

public partial class Role
{
    public decimal Id { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Useraccount> Useraccounts { get; set; } = new List<Useraccount>();
}
