namespace DATN.RequestDto
{
    public class LoHangCreateRequestDto
    {
        public Guid SanPhamId { get; set; }
        public string MaLo { get; set; } = null!;
        public DateOnly NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }
        public int? SoLuong { get; set; }
        public string? TieuChuanApDung { get; set; }
        public string? KetQuaKiemNghiem { get; set; }
        public string TrangThai { get; set; } = "ACTIVE"; // mặc định
    }

    public class LoHangUpdateRequestDto
    {
        public string? MaLo { get; set; }
        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }
        public int? SoLuong { get; set; }
        public string? TieuChuanApDung { get; set; }
        public string? KetQuaKiemNghiem { get; set; }
        public string? TrangThai { get; set; }
    }
}
