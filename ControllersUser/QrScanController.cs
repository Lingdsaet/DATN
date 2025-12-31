using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN.ControllersUser
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

            //  Tìm QR Lô hàng
            var qrLoHang = await _scanRepo.GetMaQrWithRelationsAsync(request.MaQr);

            // 2Nếu không có → tìm QR Sản phẩm
            MaQrSanPham? qrSanPham = null;
            if (qrLoHang == null)
            {
                qrSanPham = await _scanRepo.GetMaQrSanPhamAsync(request.MaQr);
            }

            // Không thuộc hệ thống
            if (qrLoHang == null && qrSanPham == null)
            {
                return Ok(new ScanQrResponseDto
                {
                    Found = false,
                    KetQua = "SUSPECT",
                    Message = "Mã QR không thuộc hệ thống.",
                    MaQr = request.MaQr
                });
            }

            var now = DateTime.UtcNow;

            // QR LÔ HÀNG 
            if (qrLoHang != null)
            {
                var lo = qrLoHang.LoHang;
                var sp = lo?.SanPham;
                var dn = sp?.DoanhNghiep;

                var ketQua = qrLoHang.TrangThai == "ACTIVE" ? "OK" : "WARNING";

                var lanQuet = new LichSuQuet
                {
                    Id = Guid.NewGuid(),
                    MaQrLoHangId = qrLoHang.Id,
                    NguoiDungId = request.NguoiDungId,
                    ThoiGian = now,
                    KetQua = ketQua,
                    ThietBi = request.ThietBi,
                    HeDieuHanh = request.HeDieuHanh,
                    ViDo = request.ViDo,
                    KinhDo = request.KinhDo,
                    DiaChiGanDung = request.DiaChiGanDung
                };

                await _scanRepo.CreateLanQuetAsync(lanQuet);

                var suKienDtos = lo == null
                    ? null
                    : (await _scanRepo.GetSuKienByLoHangIdAsync(lo.Id))
                        .Select(s => new ScanQrSuKienDto
                        {
                            ThoiGian = s.ThoiGian,
                            LoaiSuKien = s.LoaiSuKien,
                            DonViThucHien = s.DonViThucHien,
                            MoTa = s.MoTa
                        }).ToList();

                return Ok(new ScanQrResponseDto
                {
                    Found = true,
                    KetQua = ketQua,
                    Message = ketQua == "OK"
                        ? "Mã QR lô hàng hợp lệ."
                        : "Mã QR không ở trạng thái ACTIVE.",

                    LanQuetId = lanQuet.Id,
                    MaQr = qrLoHang.MaQr,
                    TrangThaiQr = qrLoHang.TrangThai,

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
                });
            }

            // QR SẢN PHẨM 
            var spQr = qrSanPham!.SanPham;
            var dnQr = spQr?.DoanhNghiep;

            var ketQuaSp = qrSanPham.TrangThai == "ACTIVE" ? "OK" : "WARNING";

            var lanQuetSp = new LichSuQuet
            {
                Id = Guid.NewGuid(),
                MaQrSanPhamId = qrSanPham.Id,
                NguoiDungId = request.NguoiDungId,
                ThoiGian = now,
                KetQua = ketQuaSp,
                ThietBi = request.ThietBi,
                HeDieuHanh = request.HeDieuHanh,
                ViDo = request.ViDo,
                KinhDo = request.KinhDo,
                DiaChiGanDung = request.DiaChiGanDung
            };

            await _scanRepo.CreateLanQuetAsync(lanQuetSp);

            return Ok(new ScanQrResponseDto
            {
                Found = true,
                KetQua = ketQuaSp,
                Message = "Mã QR sản phẩm hợp lệ.",

                LanQuetId = lanQuetSp.Id,
                MaQr = qrSanPham.MaQr,
                TrangThaiQr = qrSanPham.TrangThai,

                SanPhamId = spQr?.Id,
                TenSanPham = spQr?.Ten,
                MaSanPham = spQr?.MaSanPham,

                DoanhNghiepId = dnQr?.Id,
                TenDoanhNghiep = dnQr?.Ten
            });
        }

    }
}
