using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Deduct.Data;

[PrimaryKey("JobBarcode", "Num")]
[Table("JobKeep", Schema = "dbo")]
[Index("DocNo", "EmpCode", Name = "IX_JobKeep_4")]
public partial class JobKeep
{
    [Key]
    [StringLength(12)]
    [Unicode(false)]
    public string JobBarcode { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string DocNo { get; set; } = null!;

    public int JobType { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ListNo { get; set; } = null!;

    public int EmpCode { get; set; }

    [StringLength(8)]
    [Unicode(false)]
    public string OrderNo { get; set; } = null!;

    [StringLength(7)]
    [Unicode(false)]
    public string CustCode { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Grade { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Lotno { get; set; } = null!;

    [StringLength(13)]
    [Unicode(false)]
    public string Barcode { get; set; } = null!;

    [StringLength(14)]
    [Unicode(false)]
    public string Article { get; set; } = null!;

    [Column("FNcode")]
    [StringLength(3)]
    [Unicode(false)]
    public string Fncode { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Silver { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal TtQty { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TtlWg { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty1 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty2 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty3 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty4 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty5 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty6 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty7 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty8 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty9 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty10 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty11 { get; set; }

    [Column(TypeName = "decimal(10, 1)")]
    public decimal Qty12 { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string DocEmp { get; set; } = null!;

    public int Model { get; set; }

    public int Chunk { get; set; }

    [Column("mdate", TypeName = "datetime")]
    public DateTime Mdate { get; set; }

    [Column("username")]
    [StringLength(20)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    public bool UpStock { get; set; }

    /// <summary>
    /// แสดงลำดับที่ ของ Jobbarcode เช่น &apos;01&apos;,&apos;02&apos;
    /// </summary>
    [Key]
    [StringLength(2)]
    [Unicode(false)]
    public string Num { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? ReDate { get; set; }

    [Column("NOtQC")]
    public bool NotQc { get; set; }

    public int Runid { get; set; }

    public bool SendQc { get; set; }

    [ForeignKey("JobBarcode, Barcode, CustCode")]
    [InverseProperty("JobKeep")]
    public virtual JobDetail JobDetail { get; set; } = null!;
}
