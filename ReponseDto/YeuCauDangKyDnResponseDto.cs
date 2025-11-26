namespace DATN.ReponseDto
{
    public class YeuCauDangKyDnResponseDto
    {
        public Guid Id { get; set; }
        public string TenDoanhNghiep { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string Email { get; set; } = null!;
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string TrangThai { get; set; } = null!;
        public string? GhiChu { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
