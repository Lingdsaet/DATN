namespace DATN.ReponseDto
{
    public class DoanhNghiepResponseDto
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string TrangThai { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
