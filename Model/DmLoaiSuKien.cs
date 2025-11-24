using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("DM_LoaiSuKien")]
public partial class DmLoaiSuKien
{
    [Key]
    [StringLength(50)]
    public string Ma { get; set; } = null!;

    [StringLength(200)]
    public string MoTa { get; set; } = null!;

    [InverseProperty("LoaiSuKienNavigation")]
    public virtual ICollection<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; } = new List<SuKienChuoiCungUng>();
}
