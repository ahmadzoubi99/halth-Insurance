﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Health_Insurance.Models;

public partial class About
{
    public decimal Id { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }

    public string? Text4 { get; set; }

    public string? Text5 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile1 { get; set; }

    public string? Image1 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile2 { get; set; }

    public string? Image2 { get; set; }
    [NotMapped]
    public IFormFile? ImageFile3 { get; set; }

    public string? Image3 { get; set; }
}
