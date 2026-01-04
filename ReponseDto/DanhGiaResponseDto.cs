namespace DATN.ReponseDto
{
    public class DanhGiaResponseDto
    {
        public Guid Id { get; set; }
        public int SoSao { get; set; }
        public string? NoiDung { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TenNguoiDung { get; set; }
        public int Likes { get; set; }
    }
}
