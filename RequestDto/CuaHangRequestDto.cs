namespace DATN.RequestDto
{
    public class CuaHangCreateRequestDto
    {
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; } = null!;
        public Guid? DiaDiemId { get; set; }
        public string? LienHe { get; set; }
    }

    public class CuaHangUpdateRequestDto
    {
        public string? Ten { get; set; }
        public Guid? DiaDiemId { get; set; }
        public string? LienHe { get; set; }
    }
}
