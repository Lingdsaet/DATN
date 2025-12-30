namespace DATN.ReponseDto
{
    public class NguoiDungResponseDto
    {
        public Guid Id { get; set; }
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public string? DienThoai { get; set; }
    }

    public class NguoiDungAdminResponseDto
    {
        public Guid Id { get; set; }
        public string HoTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? DienThoai { get; set; }
        public bool KichHoat { get; set; }
        public List<string> VaiTros { get; set; } = new();
    }

}
