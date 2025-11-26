namespace DATN.ReponseDto
{
    public class CuaHangResponseDto
    {
        public Guid Id { get; set; }
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; } = null!;
        public Guid? DiaDiemId { get; set; }
        public string? LienHe { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
