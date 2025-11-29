using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("BaoCaoNguoiDung")]
[Index("TrangThaiXuLy", Name = "IX_BCND_TrangThai")]
public partial class BaoCaoNguoiDung
{
    [Key]
    public Guid Id { get; set; }

    public Guid? NguoiDungId { get; set; }

    [Column("MaQR_LoHangId")]
    public Guid? MaQrLoHangId { get; set; }

    public Guid? LanQuetId { get; set; }

    [Precision(0)]
    public DateTime ThoiGian { get; set; }

    [StringLength(30)]
    public string MucDo { get; set; } = null!;

    public string? MoTa { get; set; }

    [StringLength(500)]
    public string? HinhAnhUrl { get; set; }

    [StringLength(30)]
    public string TrangThaiXuLy { get; set; } = null!;

    [StringLength(500)]
    public string? GhiChuXuLy { get; set; }

    [ForeignKey("LanQuetId")]
    [InverseProperty("BaoCaoNguoiDungs")]
    public virtual LichSuQuet? LanQuet { get; set; }

    [ForeignKey("MaQrLoHangId")]
    [InverseProperty("BaoCaoNguoiDungs")]
    public virtual MaQrLoHang? MaQrLoHang { get; set; }

    [ForeignKey("NguoiDungId")]
    [InverseProperty("BaoCaoNguoiDungs")]
    public virtual NguoiDung? NguoiDung { get; set; }
}
