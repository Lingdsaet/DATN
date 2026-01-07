namespace DATN.RequestDto
{
    public class SuKienChuoiCungUngRequestDto
    {
        public Guid LoHangId { get; set; }
        public string LoaiSuKien { get; set; } = null!;
        public DateTime ThoiGian { get; set; }
        public string? DonViThucHien { get; set; }
        public Guid? DiaDiemId { get; set; }
        public string? TaiLieuDinhKemUrl { get; set; }
        public string? MoTa { get; set; }
    }
}
