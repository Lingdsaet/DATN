using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("DiaDiem")]
public partial class DiaDiem
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string Ten { get; set; } = null!;

    [StringLength(100)]
    public string? Tinh { get; set; }

    [StringLength(100)]
    public string? Huyen { get; set; }

    [StringLength(100)]
    public string? Xa { get; set; }

    [StringLength(300)]
    public string? DiaChi { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? ViDo { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? KinhDo { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("DiaDiem")]
    public virtual ICollection<CuaHang> CuaHangs { get; set; } = new List<CuaHang>();

    [InverseProperty("DiaDiem")]
    public virtual ICollection<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; } = new List<SuKienChuoiCungUng>();
}
