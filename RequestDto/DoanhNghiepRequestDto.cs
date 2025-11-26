namespace DATN.RequestDto
{
    public class DoanhNghiepCreateRequestDto
    {
        public string Ten { get; set; } = null!;
        public string? MaSoThue { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }

        // ACTIVE / PENDING / SUSPENDED (tuỳ bạn dùng)
        public string TrangThai { get; set; } = "ACTIVE";
    }

    public class DoanhNghiepUpdateRequestDto
    {
        public string? Ten { get; set; }
        public string? MaSoThue { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
        public string? DiaChi { get; set; }
        public string? TrangThai { get; set; }
    }
}
