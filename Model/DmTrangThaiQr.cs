using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

[Table("DM_TrangThaiQR")]
public partial class DmTrangThaiQr
{
    [Key]
    [StringLength(50)]
    public string Ma { get; set; } = null!;

    [StringLength(200)]
    public string MoTa { get; set; } = null!;

    [InverseProperty("TrangThaiNavigation")]
    public virtual ICollection<MaQrLoHang> MaQrLoHangs { get; set; } = new List<MaQrLoHang>();
}
