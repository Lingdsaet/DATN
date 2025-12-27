using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("LoaiSanPham")]
[Index("MaLoai", Name = "UQ__LoaiSanP__730A575833740039", IsUnique = true)]
public partial class LoaiSanPham
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string TenLoai { get; set; } = null!;

    [StringLength(100)]
    public string MaLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("LoaiSanPham")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
