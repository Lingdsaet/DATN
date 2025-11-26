namespace DATN.ReponseDto
{
    public class BaoCaoNguoiDungResponseDto
    {
        public Guid Id { get; set; }
        public Guid? NguoiTieuDungId { get; set; }
        public Guid? MaQrLoHangId { get; set; }
        public Guid? LanQuetId { get; set; }

        public DateTime ThoiGian { get; set; }
        public string MucDo { get; set; } = null!;
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }

        public string TrangThaiXuLy { get; set; } = null!;
        public string? GhiChuXuLy { get; set; }
    }
}
