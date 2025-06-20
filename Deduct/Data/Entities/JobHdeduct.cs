using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Data.Entities;

[Keyless]
[Table("JobHDeduct", Schema = "dbo")]
public partial class JobHdeduct
{
    [StringLength(10)]
    [Unicode(false)]
    public string SendDoc { get; set; } = null!;

    public DateOnly Senddate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? Username { get; set; }
}
