using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("MaQR_LoHang")]
[Index("MaQr", Name = "UX_MaQR_Unique", IsUnique = true)]
public partial class MaQrLoHang
{
    [Key]
    public Guid Id { get; set; }

    public Guid LoHangId { get; set; }

    [Column("MaQR")]
    [StringLength(200)]
    public string MaQr { get; set; } = null!;

    [StringLength(50)]
    public string TrangThai { get; set; } = null!;

    [StringLength(300)]
    public string? GhiChu { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("MaQrLoHang")]
    public virtual ICollection<BaoCaoNguoiDung> BaoCaoNguoiDungs { get; set; } = new List<BaoCaoNguoiDung>();

    [InverseProperty("MaQrLoHang")]
    public virtual ICollection<LichSuQuet> LichSuQuets { get; set; } = new List<LichSuQuet>();

    [ForeignKey("LoHangId")]
    [InverseProperty("MaQrLoHangs")]
    public virtual LoHang LoHang { get; set; } = null!;

    [ForeignKey("TrangThai")]
    [InverseProperty("MaQrLoHangs")]
    public virtual DmTrangThaiQr TrangThaiNavigation { get; set; } = null!;
}
