using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("NguoiDung")]
[Index("Email", Name = "IX_NguoiDung_Email", IsUnique = true)]
public partial class NguoiDung
{
    [Key]
    public Guid Id { get; set; }

    public Guid? DoanhNghiepId { get; set; }

    [StringLength(150)]
    public string HoTen { get; set; } = null!;

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [MaxLength(256)]
    public byte[]? MatKhauHash { get; set; }

    [StringLength(50)]
    public string? DienThoai { get; set; }

    public bool KichHoat { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [InverseProperty("NguoiDung")]
    public virtual ICollection<BaoCaoNguoiDung> BaoCaoNguoiDungs { get; set; } = new List<BaoCaoNguoiDung>();

    [ForeignKey("DoanhNghiepId")]
    [InverseProperty("NguoiDungs")]
    public virtual DoanhNghiep? DoanhNghiep { get; set; }

    [InverseProperty("NguoiDung")]
    public virtual ICollection<LichSuQuet> LichSuQuets { get; set; } = new List<LichSuQuet>();

    [ForeignKey("NguoiDungId")]
    [InverseProperty("NguoiDungs")]
    public virtual ICollection<VaiTro> VaiTros { get; set; } = new List<VaiTro>();
    public ICollection<NguoiDungVaiTro> NguoiDungVaiTros { get; set; }
        = new List<NguoiDungVaiTro>();

}
