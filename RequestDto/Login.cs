namespace DATN.RequestDto
{
    // Request dùng để gửi từ FE
    public class LoginRequestDto
    {
        public string EmailOrPhone { get; set; } = null!;   // nhập email hoặc sđt
        public string MatKhau { get; set; } = null!;
    }

    // Thông tin người dùng trả về sau đăng nhập
    public class LoginUserDto
    {
        public Guid Id { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }

        // Danh sách mã vai trò: ["ADMIN", "DOANH_NGHIEP", "NGUOI_TIEU_DUNG"]
        public List<string> Roles { get; set; } = new();

        // Flag tiện lợi cho FE
        public bool IsAdmin => Roles.Contains("ADMIN");
        public bool IsDoanhNghiep => Roles.Contains("DOANH_NGHIEP");
        public bool IsNguoiTieuDung => Roles.Contains("NGUOI_TIEU_DUNG");
    }

    // Wrapper chung (nếu bạn chưa có thì dùng)
    public class AuthResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    public class LoginResultDto
    {
        public LoginUserDto User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }


}
