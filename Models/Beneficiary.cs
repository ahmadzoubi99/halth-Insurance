using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Insurance.Models;

public partial class Beneficiary
{
    public decimal Id { get; set; }

    public string? BeneficiaryName { get; set; }

    public string? BeneficiaryRelation { get; set; }

    public string? BeneficiaryState { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public string? BeneficiaryImageProof { get; set; }

    public decimal? SubscriptionsId { get; set; }

    public DateTime? DateBeneficiary { get; set; }

    public virtual Subscription? Subscriptions { get; set; }
}
