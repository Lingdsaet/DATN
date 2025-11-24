using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("NguoiTieuDung")]
public partial class NguoiTieuDung
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(150)]
    public string? HoTen { get; set; }

    [StringLength(200)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? DienThoai { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [MaxLength(256)]
    public byte[]? MatKhauHash { get; set; }

    [InverseProperty("NguoiTieuDung")]
    public virtual ICollection<BaoCaoNguoiDung> BaoCaoNguoiDungs { get; set; } = new List<BaoCaoNguoiDung>();

    [InverseProperty("NguoiTieuDung")]
    public virtual ICollection<LichSuQuet> LichSuQuets { get; set; } = new List<LichSuQuet>();
}
