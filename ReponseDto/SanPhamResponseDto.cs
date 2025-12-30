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
        public decimal? Gia { get; set; }
        public int SoLuong { get; set; }
        public string? DonViTinh { get; set; }
        public Guid? LoaiSanPhamId { get; set; }
        public string? HinhAnhUrl { get; set; }
        public string? TieuChuanApDung { get; set; }
        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }
        public string? NoiSanXuat { get; set; }
        
        public string TrangThai { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? QrImageUrl { get; set; }
    }
}
