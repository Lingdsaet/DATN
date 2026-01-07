namespace DATN.RequestDto
{
    public class DiaDiemDto
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = null!;
        public string? DiaChi { get; set; }
        public decimal? ViDo { get; set; }
        public decimal? KinhDo { get; set; }
        public string? TenDoanhNghiep { get; set; }
    }

}
