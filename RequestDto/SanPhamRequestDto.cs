namespace DATN.RequestDto
{
    public class SanPhamCreateRequestDto
    {
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; } = null!;
        public string? MaSanPham { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
        public string? TieuChuanApDung { get; set; }

        // PENDING / APPROVED / REJECTED
        public string TrangThai { get; set; } = "PENDING";
    }

    public class SanPhamUpdateRequestDto
    {
        public string? Ten { get; set; }
        public string? MaSanPham { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
        public string? TieuChuanApDung { get; set; }
        public string? TrangThai { get; set; }
    }
}
