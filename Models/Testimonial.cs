using System;
using System.Collections.Generic;

namespace Health_Insurance.Models;

public partial class Testimonial
{
    public decimal Id { get; set; }

    public decimal? UseraccountId { get; set; }

    public string? TestimonialText { get; set; }

    public DateTime? TestimonialDate { get; set; }

    public string? Status { get; set; }

    public virtual Useraccount? Useraccount { get; set; }
}
