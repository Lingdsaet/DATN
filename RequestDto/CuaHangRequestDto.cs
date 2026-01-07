namespace DATN.RequestDto
{
    public class CuaHangRequestDto
    {
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; } = null!;
        public Guid? DiaDiemId { get; set; }
        public string? LienHe { get; set; }
    }
    public class CuaHangCreateRequestDto
    {
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; } = null!;
        public string? LienHe { get; set; }
        public DiaDiemCreateDto DiaDiem { get; set; } = null!;
    }
    public class DiaDiemCreateDto
    {
        public string Ten { get; set; } = null!;
        public string? Tinh { get; set; }
        public string? Huyen { get; set; }
        public string? Xa { get; set; }
        public string? DiaChi { get; set; }
        public decimal? ViDo { get; set; }
        public decimal? KinhDo { get; set; }
    }

}
