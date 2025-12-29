using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DATN.Model;

namespace DATN.Model;

[Table("SanPham")]
[Index("LoaiSanPhamId", Name = "IX_SanPham_LoaiSanPhamId")]
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

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Gia { get; set; }

    public int SoLuong { get; set; }

    [StringLength(50)]
    public string? DonViTinh { get; set; }

    public DateOnly? NgaySanXuat { get; set; }

    public DateOnly? HanSuDung { get; set; }

    [StringLength(200)]
    public string? NoiSanXuat { get; set; }

    [Column("QRCode")]
    [StringLength(300)]
    public string? Qrcode { get; set; }

    public Guid? LoaiSanPhamId { get; set; }

    [ForeignKey("DoanhNghiepId")]
    [InverseProperty("SanPhams")]
    public virtual DoanhNghiep DoanhNghiep { get; set; } = null!;

    [InverseProperty("SanPham")]
    public virtual ICollection<LoHang> LoHangs { get; set; } = new List<LoHang>();

    [ForeignKey("LoaiSanPhamId")]
    [InverseProperty("SanPhams")]
    public virtual LoaiSanPham? LoaiSanPham { get; set; }

    [InverseProperty("SanPham")]
    public virtual ICollection<MaQrSanPham> MaQrSanPhams { get; set; } = new List<MaQrSanPham>();
}
