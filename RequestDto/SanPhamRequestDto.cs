namespace DATN.RequestDto
{
    public class CreateSanPhamDto
    {
        public Guid DoanhNghiepId { get; set; }
        public Guid? LoaiSanPhamId { get; set; }

        public string Ten { get; set; } = null!;
        public string? MaSanPham { get; set; }
        public string? MoTa { get; set; }
        public string? TieuChuanApDung { get; set; }

        public decimal? Gia { get; set; }
        public int SoLuong { get; set; }
        public string? DonViTinh { get; set; }

        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }

        public string? NoiSanXuat { get; set; }

        public IFormFile? HinhAnh { get; set; }
    }



    public class SanPhamUpdateRequestDto
    {
        public string? Ten { get; set; }
        public string? MaSanPham { get; set; }
        public string? MoTa { get; set; }
        public string? TieuChuanApDung { get; set; }

        public decimal? Gia { get; set; }
        public int? SoLuong { get; set; }
        public string? DonViTinh { get; set; }

        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }
        public string? NoiSanXuat { get; set; }

        public string? HinhAnhUrl { get; set; }
        public Guid? LoaiSanPhamId { get; set; }

        public string? TrangThai { get; set; }
    }

    public class SanPhamSearchRequest
    {
        public string? Keyword { get; set; }
    }

}
