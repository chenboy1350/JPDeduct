using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Data.Entities;

[Keyless]
[Table("JobDeduct", Schema = "dbo")]
public partial class JobDeduct
{
    public int RunDeduct { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    public string Docno { get; set; } = null!;

    public int EmpCode { get; set; }

    [Column("num")]
    [StringLength(2)]
    [Unicode(false)]
    public string Num { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string Jobbarcode { get; set; } = null!;

    public int DeductQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal DeductWg { get; set; }

    [Column("mDate", TypeName = "datetime")]
    public DateTime MDate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string UserId { get; set; } = null!;

    public bool SendJob { get; set; }

    [Column("Doc_no")]
    [StringLength(11)]
    [Unicode(false)]
    public string? DocNo { get; set; }

    [Column("item")]
    public int? Item { get; set; }

    [Column(TypeName = "money")]
    public decimal? Amount { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? R1 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? R2 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? R3 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? R4 { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? R5 { get; set; }

    [Column("RDATE")]
    public DateOnly? Rdate { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string? SendDoc { get; set; }
}
