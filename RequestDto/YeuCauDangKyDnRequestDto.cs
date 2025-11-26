namespace DATN.RequestDto
{
    public class YeuCauDangKyDnCreateRequestDto
    {
        public string TenDoanhNghiep { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string Email { get; set; } = null!;
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
    }

    public class YeuCauDangKyDnUpdateStatusRequestDto
    {
        // PENDING / APPROVED / REJECTED
        public string TrangThai { get; set; } = null!;
        public string? GhiChu { get; set; }
    }
}
