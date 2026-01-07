using DATN.RequestDto;

namespace DATN.ReponseDto
{
    public class CuaHangResponseDto
    {
        public Guid Id { get; set; }
        public string? TenDoanhNghiep{ get; set; }
        public string Ten { get; set; } = null!;
        public DiaDiemDto? DiaDiem { get; set; }
        public string? LienHe { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
