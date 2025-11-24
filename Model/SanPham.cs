using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("SanPham")]
[Index("DoanhNghiepId", "Ten", Name = "IX_SanPham_Ten_Per_DN", IsUnique = true)]
public partial class SanPham
{
    [Key]
    public Guid Id { get; set; }

    public Guid DoanhNghiepId { get; set; }

    [StringLength(200)]
    public string Ten { get; set; } = null!;

    [StringLength(100)]
    public string? MaSanPham { get; set; }

    public string? MoTa { get; set; }

    [StringLength(500)]
    public string? HinhAnhUrl { get; set; }

    [StringLength(300)]
    public string? TieuChuanApDung { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [ForeignKey("DoanhNghiepId")]
    [InverseProperty("SanPhams")]
    public virtual DoanhNghiep DoanhNghiep { get; set; } = null!;

    [InverseProperty("SanPham")]
    public virtual ICollection<LoHang> LoHangs { get; set; } = new List<LoHang>();
}
