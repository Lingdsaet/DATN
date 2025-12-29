using System.ComponentModel.DataAnnotations;

namespace DATN.RequestDto
{
    public class LoaiSanPhamCreateRequestDto
    {
        [Required]
        [StringLength(200)]
        public string TenLoai { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string MaLoai { get; set; } = null!;

        public string? MoTa { get; set; }
    }
    public class LoaiSanPhamUpdateRequestDto
    {
        public string? TenLoai { get; set; }
        public string? MoTa { get; set; }
        public string? TrangThai { get; set; }
    }

}
