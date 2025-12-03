namespace DATN.ReponseDto
{
    public class SanPhamResponseDto
    {
        public Guid Id { get; set; }
        public Guid DoanhNghiepId { get; set; }
        public string? TenDoanhNghiep { get; set; }
        public string Ten { get; set; } = null!;
        public string? MaSanPham { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
        public string? TieuChuanApDung { get; set; }
        public string TrangThai { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
