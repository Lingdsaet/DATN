using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("LoHang")]
[Index("SanPhamId", "MaLo", Name = "IX_LoHang_MaLo_Per_SanPham", IsUnique = true)]
public partial class LoHang
{
    [Key]
    public Guid Id { get; set; }

    public Guid SanPhamId { get; set; }

    [StringLength(100)]
    public string MaLo { get; set; } = null!;

    public DateOnly NgaySanXuat { get; set; }

    public DateOnly? HanSuDung { get; set; }

    public int? SoLuong { get; set; }

    [StringLength(300)]
    public string? TieuChuanApDung { get; set; }

    public string? KetQuaKiemNghiem { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("LoHang")]
    public virtual ICollection<MaQrLoHang> MaQrLoHangs { get; set; } = new List<MaQrLoHang>();

    [ForeignKey("SanPhamId")]
    [InverseProperty("LoHangs")]
    public virtual SanPham SanPham { get; set; } = null!;

    [InverseProperty("LoHang")]
    public virtual ICollection<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; } = new List<SuKienChuoiCungUng>();
}
