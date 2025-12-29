namespace DATN.ReponseDto
{
    public class LoaiSanPhamResponseDto
    {
        public Guid Id { get; set; }
        public string TenLoai { get; set; } = null!;
        public string MaLoai { get; set; } = null!;
        public string? MoTa { get; set; }
        public string TrangThai { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

}
