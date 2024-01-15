using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Insurance.Models;

public partial class Useraccount
{
    public decimal Id { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string? Passwd { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public decimal? RolesId { get; set; }

    public string? Imagepath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
    public virtual Role? Roles { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
