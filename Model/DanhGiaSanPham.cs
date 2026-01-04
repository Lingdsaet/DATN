using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    namespace DATN.Model
    {
        [Table("DanhGiaSanPham")]
        [Index(nameof(SanPhamId))]
        [Index(nameof(NguoiDungId))]
        public class DanhGiaSanPham
        {
            [Key]
            public Guid Id { get; set; }

            [Required]
            public Guid SanPhamId { get; set; }

            public Guid? NguoiDungId { get; set; }

            [Required]
            public int SoSao { get; set; }     // 1–5

            [StringLength(1000)]
            public string? NoiDung { get; set; }

            public int Likes { get; set; } = 0;

            [Precision(0)]
            public DateTime CreatedAt { get; set; }

            [Precision(0)]
            public DateTime UpdatedAt { get; set; }

            public bool XoaMem { get; set; } = false;

            [ForeignKey(nameof(SanPhamId))]
            public virtual SanPham SanPham { get; set; } = null!;

            [ForeignKey(nameof(NguoiDungId))]
            public virtual NguoiDung? NguoiDung { get; set; }
        }
    }


}
