using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("YeuCauDangKyDN")]
public partial class YeuCauDangKyDn
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(200)]
    public string TenDoanhNghiep { get; set; } = null!;

    [StringLength(50)]
    public string? MaSoThue { get; set; }

    [StringLength(200)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string? DienThoai { get; set; }

    [StringLength(300)]
    public string? DiaChi { get; set; }

    [StringLength(30)]
    public string TrangThai { get; set; } = null!;

    [StringLength(500)]
    public string? GhiChu { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }
}
