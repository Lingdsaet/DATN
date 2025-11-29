namespace DATN.RequestDto
{
    public class ScanQrRequestDto
    {
        public string MaQr { get; set; } = null!;

        public Guid? NguoiDungId { get; set; }

        public string? ThietBi { get; set; }
        public string? HeDieuHanh { get; set; }

        public decimal? ViDo { get; set; }
        public decimal? KinhDo { get; set; }
        public string? DiaChiGanDung { get; set; }
    }
}
