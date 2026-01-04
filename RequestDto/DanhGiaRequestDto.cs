namespace DATN.RequestDto
{
    public class DanhGiaRequestDto
    {
        public Guid SanPhamId { get; set; }
        public Guid? NguoiDungId { get; set; }
        public int SoSao { get; set; }
        public string? NoiDung { get; set; }
    }
}
