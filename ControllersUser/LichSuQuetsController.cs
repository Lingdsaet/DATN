using DATN.ReponseDto;
using DATN.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class LichSuQuetController : ControllerBase
    {
        private readonly ILichSuQuetRepository _lichSuQuetRepo;

        public LichSuQuetController(ILichSuQuetRepository lichSuQuetRepo)
        {
            _lichSuQuetRepo = lichSuQuetRepo;
        }

        // GET: api/LichSuQuet/nguoi-tieu-dung/{nguoiTieuDungId}
        [HttpGet("nguoi-tieu-dung/{nguoiTieuDungId}")]
        public async Task<ActionResult<IEnumerable<LichSuQuetItemDto>>> GetByNguoiTieuDungId(Guid nguoiTieuDungId)
        {
            var list = await _lichSuQuetRepo.GetByNguoiTieuDungIdAsync(nguoiTieuDungId);

            var result = list.Select(x =>
            {
                var qrLo = x.MaQrLoHang;
                var qrSp = x.MaQrSanPham;

                var lo = qrLo?.LoHang;
                var sp = lo?.SanPham ?? qrSp?.SanPham;

                return new LichSuQuetItemDto
                {
                    Id = x.Id,
                    MaQR = qrLo?.MaQr ?? qrSp?.MaQr ?? string.Empty,
                    ThoiGian = x.ThoiGian,
                    KetQua = x.KetQua,
                    TenSanPham = sp?.Ten
                };
            });

            return Ok(result);
        }


        // GET: api/LichSuQuet/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LichSuQuetDetailDto>> GetById(Guid id)
        {
            var x = await _lichSuQuetRepo.GetByIdAsync(id);
            if (x == null) return NotFound();

            var qrLo = x.MaQrLoHang;
            var qrSp = x.MaQrSanPham;

            var lo = qrLo?.LoHang;
            var sp = lo?.SanPham ?? qrSp?.SanPham;
            var dn = sp?.DoanhNghiep;

            var result = new LichSuQuetDetailDto
            {
                Id = x.Id,
                MaQR = qrLo?.MaQr ?? qrSp?.MaQr ?? string.Empty,

                ThoiGian = x.ThoiGian,
                KetQua = x.KetQua,
                ThietBi = x.ThietBi,
                HeDieuHanh = x.HeDieuHanh,
                ViDo = x.ViDo,
                KinhDo = x.KinhDo,
                DiaChiGanDung = x.DiaChiGanDung,
                NguoiDungId = x.NguoiDungId,

                TenSanPham = sp?.Ten,
                MaSanPham = sp?.MaSanPham,

                MaLo = lo?.MaLo,
                NgaySanXuat = lo?.NgaySanXuat,
                HanSuDung = lo?.HanSuDung,

                TenDoanhNghiep = dn?.Ten,
                EmailDoanhNghiep = dn?.Email,
                DienThoaiDoanhNghiep = dn?.DienThoai
            };

            return Ok(result);
        }
    }
    }
