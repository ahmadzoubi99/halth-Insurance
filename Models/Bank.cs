using System;
using System.Collections.Generic;

namespace Health_Insurance.Models;

public partial class Bank
{
    public decimal Id { get; set; }

    public string? OwnerName { get; set; }

    public decimal? CardNumber { get; set; }

    public decimal? Balance { get; set; }

    public decimal? Cvv { get; set; }

    public decimal? ExpirationDateMonth { get; set; }

    public decimal? ExpirationDateYear { get; set; }
}
