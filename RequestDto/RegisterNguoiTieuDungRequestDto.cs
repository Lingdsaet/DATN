namespace DATN.RequestDto
{
    public class RegisterNguoiTieuDungRequest
    {
        public string? HoTen { get; set; }

        public string? Email { get; set; }

        public string? DienThoai { get; set; }

        public string MatKhau { get; set; } = null!;

        public string XacNhanMatKhau { get; set; } = null!;
    }

}
