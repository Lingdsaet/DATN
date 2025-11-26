using System;
using System.Collections.Generic;

namespace DATN.ReponseDto
{
    public class ScanQrResponseDto
    {
        public bool Found { get; set; }
        public string KetQua { get; set; } = null!;      // OK / WARNING / SUSPECT
        public string? Message { get; set; }

        public Guid? LanQuetId { get; set; }

        // QR
        public string? MaQr { get; set; }
        public string? TrangThaiQr { get; set; }

        // Lô hàng
        public Guid? LoHangId { get; set; }
        public string? MaLo { get; set; }
        public DateOnly? NgaySanXuat { get; set; }
        public DateOnly? HanSuDung { get; set; }

        // Sản phẩm
        public Guid? SanPhamId { get; set; }
        public string? TenSanPham { get; set; }
        public string? MaSanPham { get; set; }

        // Doanh nghiệp
        public Guid? DoanhNghiepId { get; set; }
        public string? TenDoanhNghiep { get; set; }

        // Chuỗi cung ứng
        public List<ScanQrSuKienDto>? SuKienChuoiCungUng { get; set; }
    }
}
