using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("VaiTro")]
[Index("Ma", Name = "UQ__VaiTro__3214CC9E3C276059", IsUnique = true)]
public partial class VaiTro
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string Ma { get; set; } = null!;

    [StringLength(100)]
    public string Ten { get; set; } = null!;

    [StringLength(250)]
    public string? MoTa { get; set; }

    [ForeignKey("VaiTroId")]
    [InverseProperty("VaiTros")]
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
