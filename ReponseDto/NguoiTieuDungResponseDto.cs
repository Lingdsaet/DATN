namespace DATN.ReponseDto
{
    public class NguoiTieuDungResponseDto
    {
        public Guid Id { get; set; }
        public string? HoTen { get; set; }
        public string Email { get; set; } = null!;
        public string? DienThoai { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
