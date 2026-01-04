namespace DATN.RequestDto
{
    public class NguoiDungCreateRequest
    {
        public Guid? DoanhNghiepId { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? DienThoai { get; set; }
        public bool KichHoat { get; set; }
    }
    public class GanVaiTroRequest
    {
        public Guid NguoiDungId { get; set; }
        public List<byte> VaiTroIds { get; set; } = new();
    }
    public class NguoiDungUpdateDto
    {
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
    }

    public class DoiMatKhauDto
    {
        public string MatKhauCu { get; set; } = null!;
        public string MatKhauMoi { get; set; } = null!;
    }

}
