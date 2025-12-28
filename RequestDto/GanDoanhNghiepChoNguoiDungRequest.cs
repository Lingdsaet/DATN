namespace DATN.RequestDto
{
    public class ThemCuaHangChoNguoiDungRequest
    {
        public Guid NguoiDungId { get; set; }

        public string TenDoanhNghiep { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? DiaChi { get; set; }
        public string? DienThoai { get; set; }
        public string? Email { get; set; }
    }
}
