using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN1.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrScanController : ControllerBase
    {
        private readonly IQrScanRepository _scanRepo;

        public QrScanController(IQrScanRepository scanRepo)
        {
            _scanRepo = scanRepo;
        }

        // POST: api/QrScan
        // Body: ScanQrRequestDto
        [HttpPost]
        public async Task<ActionResult<ScanQrResponseDto>> Scan([FromBody] ScanQrRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var qrEntity = await _scanRepo.GetMaQrWithRelationsAsync(request.MaQr);

            // Không tìm thấy QR trong hệ thống
            if (qrEntity == null)
            {
                var notFoundResponse = new ScanQrResponseDto
                {
                    Found = false,
                    KetQua = "SUSPECT",
                    Message = "Mã QR không thuộc hệ thống.",
                    MaQr = request.MaQr
                };
                return Ok(notFoundResponse);
            }

            var lo = qrEntity.LoHang;
            var sp = lo?.SanPham;
            var dn = sp?.DoanhNghiep;

            var now = DateTime.UtcNow;

            // Xác định kết quả
            var ketQua = qrEntity.TrangThai == "ACTIVE" ? "OK" : "WARNING";

            // Ghi lịch sử quét
            var lanQuet = new LichSuQuet
            {
                Id = Guid.NewGuid(),
                MaQrLoHangId = qrEntity.Id,
                NguoiTieuDungId = request.NguoiTieuDungId,
                ThoiGian = now,
                KetQua = ketQua,                // OK/WARNING/SUSPECT
                ThietBi = request.ThietBi,
                HeDieuHanh = request.HeDieuHanh,
                ViDo = request.ViDo,
                KinhDo = request.KinhDo,
                DiaChiGanDung = request.DiaChiGanDung
            };

            lanQuet = await _scanRepo.CreateLanQuetAsync(lanQuet);

            // Lấy sự kiện chuỗi cung ứng
            List<ScanQrSuKienDto>? suKienDtos = null;
            if (lo != null)
            {
                var suKienList = await _scanRepo.GetSuKienByLoHangIdAsync(lo.Id);
                suKienDtos = suKienList.Select(s => new ScanQrSuKienDto
                {
                    ThoiGian = s.ThoiGian,
                    LoaiSuKien = s.LoaiSuKien,
                    DonViThucHien = s.DonViThucHien,
                    DiaChi = null, // có thể map từ DiaDiem nếu Include thêm
                    MoTa = s.MoTa
                }).ToList();
            }

            var response = new ScanQrResponseDto
            {
                Found = true,
                KetQua = ketQua,
                Message = ketQua == "OK"
                    ? "Mã QR hợp lệ."
                    : "Mã QR không ở trạng thái ACTIVE.",
                LanQuetId = lanQuet.Id,

                MaQr = qrEntity.MaQr,
                TrangThaiQr = qrEntity.TrangThai,

                LoHangId = lo?.Id,
                MaLo = lo?.MaLo,
                NgaySanXuat = lo?.NgaySanXuat,
                HanSuDung = lo?.HanSuDung,

                SanPhamId = sp?.Id,
                TenSanPham = sp?.Ten,
                MaSanPham = sp?.MaSanPham,

                DoanhNghiepId = dn?.Id,
                TenDoanhNghiep = dn?.Ten,

                SuKienChuoiCungUng = suKienDtos
            };

            return Ok(response);
        }
    }
}
