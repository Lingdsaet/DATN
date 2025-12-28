using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("SuKienChuoiCungUng")]
[Index("LoHangId", "ThoiGian", Name = "IX_SKCCU_LoHang_ThoiGian")]
public partial class SuKienChuoiCungUng
{
    [Key]
    public Guid Id { get; set; }

    public Guid LoHangId { get; set; }

    [StringLength(50)]
    public string LoaiSuKien { get; set; } = null!;

    [Precision(0)]
    public DateTime ThoiGian { get; set; }

    [StringLength(200)]
    public string? DonViThucHien { get; set; }

    public Guid? DiaDiemId { get; set; }

    [StringLength(500)]
    public string? TaiLieuDinhKemUrl { get; set; }

    public string? MoTa { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [ForeignKey("DiaDiemId")]
    [InverseProperty("SuKienChuoiCungUngs")]
    public virtual DiaDiem? DiaDiem { get; set; }

    [ForeignKey("LoHangId")]
    [InverseProperty("SuKienChuoiCungUngs")]
    public virtual LoHang LoHang { get; set; } = null!;

    [ForeignKey("LoaiSuKien")]
    [InverseProperty("SuKienChuoiCungUngs")]
    public virtual LoaiSuKien LoaiSuKienNavigation { get; set; } = null!;
}
