namespace DATN.ReponseDto
{
    public class ScanQrSuKienDto
    {
        public DateTime ThoiGian { get; set; }
        public string LoaiSuKien { get; set; } = null!;
        public string? DonViThucHien { get; set; }
        public string? DiaChi { get; set; }
        public string? MoTa { get; set; }
    }
}
