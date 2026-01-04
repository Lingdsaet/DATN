using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model
{
    [Table("TinTuc")]
    public class TinTuc
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(300)]
        public string TieuDe { get; set; } = null!;

        [StringLength(500)]
        public string? TomTat { get; set; }

        [Required]
        public string NoiDung { get; set; } = null!;

        [StringLength(500)]
        public string? HinhAnhUrl { get; set; }

        public bool NoiBat { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool XoaMem { get; set; }
    }

}
