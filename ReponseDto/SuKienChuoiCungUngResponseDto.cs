namespace DATN.ReponseDto
{
    public class SuKienChuoiCungUngResponseDto
    {
        public Guid Id { get; set; }
        public Guid LoHangId { get; set; }
        public string LoaiSuKien { get; set; } = null!;
        public string? TenLoaiSuKien { get; set; }

        public DateTime ThoiGian { get; set; }
        public string? DonViThucHien { get; set; }

        public Guid? DiaDiemId { get; set; }
        public string? TenDiaDiem { get; set; }

        public string? TaiLieuDinhKemUrl { get; set; }
        public string? MoTa { get; set; }
    }

}
