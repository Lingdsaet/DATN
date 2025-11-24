using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("DoanhNghiep")]
[Index("Ten", Name = "IX_DoanhNghiep_Ten", IsUnique = true)]
public partial class DoanhNghiep
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string Ten { get; set; } = null!;

    [StringLength(50)]
    public string? MaSoThue { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? DienThoai { get; set; }

    [StringLength(300)]
    public string? DiaChi { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("DoanhNghiep")]
    public virtual ICollection<CuaHang> CuaHangs { get; set; } = new List<CuaHang>();

    [InverseProperty("DoanhNghiep")]
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();

    [InverseProperty("DoanhNghiep")]
    public virtual ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
}
