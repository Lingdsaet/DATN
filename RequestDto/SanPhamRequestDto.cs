namespace DATN.RequestDto
{
    public class CreateSanPhamDto
    {
        public Guid DoanhNghiepId { get; set; }
        public string Ten { get; set; }
        public string MaSanPham { get; set; }
        public string MoTa { get; set; }
        public string TieuChuanApDung { get; set; }

        // Ảnh upload
        public IFormFile? HinhAnh { get; set; }
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
