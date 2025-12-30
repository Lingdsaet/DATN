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

}
