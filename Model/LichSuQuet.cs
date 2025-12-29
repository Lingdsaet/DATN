using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model;

[Table("LichSuQuet")]
[Index("MaQrLoHangId", "ThoiGian", Name = "IX_LanQuet_QR_ThoiGian")]
public partial class LichSuQuet
{
    [Key]
    public Guid Id { get; set; }

    [Column("MaQR_LoHangId")]
    public Guid? MaQrLoHangId { get; set; }

    public Guid? NguoiDungId { get; set; }

    [Precision(0)]
    public DateTime ThoiGian { get; set; }

    [StringLength(30)]
    public string KetQua { get; set; } = null!;

    [StringLength(150)]
    public string? ThietBi { get; set; }

    [StringLength(80)]
    public string? HeDieuHanh { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? ViDo { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal? KinhDo { get; set; }

    [StringLength(300)]
    public string? DiaChiGanDung { get; set; }

    [Column("MaQR_SanPhamId")]
    public Guid? MaQrSanPhamId { get; set; }

    [InverseProperty("LanQuet")]
    public virtual ICollection<BaoCaoNguoiDung> BaoCaoNguoiDungs { get; set; } = new List<BaoCaoNguoiDung>();

    [ForeignKey("MaQrLoHangId")]
    [InverseProperty("LichSuQuets")]
    public virtual MaQrLoHang? MaQrLoHang { get; set; }

    [ForeignKey("MaQrSanPhamId")]
    [InverseProperty("LichSuQuets")]
    public virtual MaQrSanPham? MaQrSanPham { get; set; }

    [ForeignKey("NguoiDungId")]
    [InverseProperty("LichSuQuets")]
    public virtual NguoiDung? NguoiDung { get; set; }
}
