using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("CuaHang")]
public partial class CuaHang
{
    [Key]
    public Guid Id { get; set; }

    public Guid DoanhNghiepId { get; set; }

    [StringLength(200)]
    public string Ten { get; set; } = null!;

    public Guid? DiaDiemId { get; set; }

    [StringLength(200)]
    public string? LienHe { get; set; }

    [Precision(0)]
    public DateTime CreatedAt { get; set; }

    [Precision(0)]
    public DateTime UpdatedAt { get; set; }

    public bool XoaMem { get; set; }

    [ForeignKey("DiaDiemId")]
    [InverseProperty("CuaHangs")]
    public virtual DiaDiem? DiaDiem { get; set; }

    [ForeignKey("DoanhNghiepId")]
    [InverseProperty("CuaHangs")]
    public virtual DoanhNghiep DoanhNghiep { get; set; } = null!;
}
