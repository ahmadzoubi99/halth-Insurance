using System;
using System.Collections.Generic;

namespace Health_Insurance.Models;

public partial class Subscription
{
    public decimal Id { get; set; }

    public decimal? UserAccountId { get; set; }

    public DateTime? SubscriptionDate { get; set; }

    public string? SubscriptionsStatus { get; set; }

    public byte? SubscriptionDuration { get; set; }

    public decimal? SubscriptionAmount { get; set; }

    public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

    public virtual Useraccount? UserAccount { get; set; }
}
