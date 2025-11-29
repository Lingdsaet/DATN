using System;

namespace DATN.ReponseDto
{
    public class LichSuQuetDetailDto
    {
        public Guid Id { get; set; }
        public string MaQR { get; set; } = null!;
        public DateTime ThoiGian { get; set; }
        public string KetQua { get; set; } = null!;
        public string? ThietBi { get; set; }
        public string? HeDieuHanh { get; set; }
        public decimal? ViDo { get; set; }
        public decimal? KinhDo { get; set; }
        public string? DiaChiGanDung { get; set; }

        public Guid? NguoiDungId { get; set; }

        // Sản phẩm & lô
        public string? TenSanPham { get; set; }
        public string? MaSanPham { get; set; }
        public string? MaLo { get; set; }
        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }

        // Doanh nghiệp
        public string? TenDoanhNghiep { get; set; }
        public string? EmailDoanhNghiep { get; set; }
        public string? DienThoaiDoanhNghiep { get; set; }
    }
}
