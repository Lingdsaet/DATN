using DATN.ReponseDto;
using DATN.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DATN1.ControllersUser
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
                var qr = x.MaQrLoHang;
                var lo = qr?.LoHang;
                var sp = lo?.SanPham;
                var dn = sp?.DoanhNghiep;

                return new LichSuQuetItemDto
                {
                    Id = x.Id,
                    MaQR = qr?.MaQr ?? string.Empty, // fixed: only use MaQr
                    ThoiGian = x.ThoiGian,
                    KetQua = x.KetQua,
                  
                };
            });

            return Ok(result);
        }

        // GET: api/LichSuQuet/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<LichSuQuetDetailDto>> GetById(Guid id)
        {
            var x = await _lichSuQuetRepo.GetByIdAsync(id);

            if (x == null)
                return NotFound();

            var qr = x.MaQrLoHang;
            var lo = qr?.LoHang;
            var sp = lo?.SanPham;
            var dn = sp?.DoanhNghiep;

            var result = new LichSuQuetDetailDto
            {
                Id = x.Id,
                MaQR = qr?.MaQr ?? string.Empty, // fixed: only use MaQr
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
