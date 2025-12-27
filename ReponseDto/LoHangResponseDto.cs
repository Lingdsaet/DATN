namespace DATN.ReponseDto
{
    public class LoHangResponseDto
    {
        public Guid Id { get; set; }
        public Guid SanPhamId { get; set; }
        public string MaLo { get; set; } = null!;
        public DateOnly NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }
        public int? SoLuong { get; set; }
        public string? TieuChuanApDung { get; set; }
        public string? KetQuaKiemNghiem { get; set; }
        public string TrangThai { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? QrImageUrl { get; set; }   

    }
    public class LoHangQrListResponseDto
    {
        public Guid LoHangId { get; set; }
        public string MaLo { get; set; } = null!;
        public string? QrImageUrl { get; set; }
    }

}
