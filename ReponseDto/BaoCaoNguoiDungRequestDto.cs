namespace DATN.ReponseDto
{
    public class BaoCaoNguoiDungCreateRequestDto
    {
        public Guid? NguoiTieuDungId { get; set; }

        // Chỉ cần 1 trong 2: MaQrLoHangId hoặc LanQuetId
        public Guid? MaQrLoHangId { get; set; }
        public Guid? LanQuetId { get; set; }

        // NGHI_VAN / THAP / TRUNG_BINH / CAO
        public string MucDo { get; set; } = "NGHI_VAN";

        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
    }
}
