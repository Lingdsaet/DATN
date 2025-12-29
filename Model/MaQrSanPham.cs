using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model;

[Table("MaQR_SanPham")]
public partial class MaQrSanPham
{
    [Key]
    public Guid Id { get; set; }

    public Guid SanPhamId { get; set; }

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

    [StringLength(500)]
    public string? QrImageUrl { get; set; }

    [InverseProperty("MaQrSanPham")]
    public virtual ICollection<LichSuQuet> LichSuQuets { get; set; } = new List<LichSuQuet>();

    [ForeignKey("SanPhamId")]
    [InverseProperty("MaQrSanPhams")]
    public virtual SanPham SanPham { get; set; } = null!;
}
