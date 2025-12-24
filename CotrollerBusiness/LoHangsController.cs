using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN1.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoHangsController : ControllerBase
    {
        private readonly ILoHangRepository _loHangRepo;
        private readonly IMaQrLoHangRepository _maQrRepo;
        private readonly QR_DATNContext _context;

        public LoHangsController(
            ILoHangRepository loHangRepo,
            IMaQrLoHangRepository maQrRepo,
            QR_DATNContext context)
        {
            _loHangRepo = loHangRepo;
            _maQrRepo = maQrRepo;
            _context = context;
        }

        // GET: api/LoHangs  (Lấy danh sách lô hàng)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoHangResponseDto>>> GetAll()
        {
            var entities = await _context.LoHangs
                .Where(x => !x.XoaMem)
                .ToListAsync();

            var dtos = entities.Select(entity => new LoHangResponseDto
            {
                Id = entity.Id,
                SanPhamId = entity.SanPhamId,
                MaLo = entity.MaLo,
                NgaySanXuat = entity.NgaySanXuat,
                HanSuDung = entity.HanSuDung,
                SoLuong = entity.SoLuong,
                TieuChuanApDung = entity.TieuChuanApDung,
                KetQuaKiemNghiem = entity.KetQuaKiemNghiem,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            }).ToList();

            return Ok(dtos);
        }

        // GET: api/LoHangs/{id}  (Xem chi tiết lô hàng)
        [HttpGet("{id}")]
        public async Task<ActionResult<LoHangResponseDto>> GetById(Guid id)
        {
            var entity = await _loHangRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy lô hàng.");

            var dto = new LoHangResponseDto
            {
                Id = entity.Id,
                SanPhamId = entity.SanPhamId,
                MaLo = entity.MaLo,
                NgaySanXuat = entity.NgaySanXuat,
                HanSuDung = entity.HanSuDung,
                SoLuong = entity.SoLuong,
                TieuChuanApDung = entity.TieuChuanApDung,
                KetQuaKiemNghiem = entity.KetQuaKiemNghiem,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return Ok(dto);
        }

        // POST: api/LoHangs  (Thêm lô hàng + tự tạo QR)
        [HttpPost]
        public async Task<ActionResult<LoHangResponseDto>> Create([FromBody] LoHangCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra sản phẩm tồn tại
            var sanPhamExists = await _context.SanPhams
                .AnyAsync(x => x.Id == request.SanPhamId && !x.XoaMem);

            if (!sanPhamExists)
                return BadRequest("Sản phẩm không tồn tại.");

            if (request.HanSuDung.HasValue && request.HanSuDung < request.NgaySanXuat)
                return BadRequest("Hạn sử dụng phải >= ngày sản xuất.");

            var now = DateTime.UtcNow;

            var entity = new LoHang
            {
                Id = Guid.NewGuid(),
                SanPhamId = request.SanPhamId,
                MaLo = request.MaLo,
                NgaySanXuat = request.NgaySanXuat,
                HanSuDung = request.HanSuDung,
                SoLuong = request.SoLuong,
                TieuChuanApDung = request.TieuChuanApDung,
                KetQuaKiemNghiem = request.KetQuaKiemNghiem,
                TrangThai = request.TrangThai,
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            // Lưu lô hàng
            entity = await _loHangRepo.CreateAsync(entity);

            // ✅ Tự động tạo QR cho lô hàng vừa thêm
            await _maQrRepo.CreateForLoHangAsync(entity.Id, "QR tự động tạo khi thêm lô hàng.");

            var dto = new LoHangResponseDto
            {
                Id = entity.Id,
                SanPhamId = entity.SanPhamId,
                MaLo = entity.MaLo,
                NgaySanXuat = entity.NgaySanXuat,
                HanSuDung = entity.HanSuDung,
                SoLuong = entity.SoLuong,
                TieuChuanApDung = entity.TieuChuanApDung,
                KetQuaKiemNghiem = entity.KetQuaKiemNghiem,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/LoHangs/{id}  (Chỉnh sửa lô hàng)
        [HttpPut("{id}")]
        public async Task<ActionResult<LoHangResponseDto>> Update(
            Guid id,
            [FromBody] LoHangUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _loHangRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy lô hàng.");

            if (!string.IsNullOrWhiteSpace(request.MaLo))
                entity.MaLo = request.MaLo;

            if (request.NgaySanXuat.HasValue)
                entity.NgaySanXuat = request.NgaySanXuat.Value;

            if (request.HanSuDung.HasValue)
                entity.HanSuDung = request.HanSuDung;

            if (entity.HanSuDung.HasValue && entity.HanSuDung < entity.NgaySanXuat)
                return BadRequest("Hạn sử dụng phải >= ngày sản xuất.");

            if (request.SoLuong.HasValue)
                entity.SoLuong = request.SoLuong;

            if (request.TieuChuanApDung != null)
                entity.TieuChuanApDung = request.TieuChuanApDung;

            if (request.KetQuaKiemNghiem != null)
                entity.KetQuaKiemNghiem = request.KetQuaKiemNghiem;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                entity.TrangThai = request.TrangThai;

            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _loHangRepo.UpdateAsync(entity);
            if (entity == null)
                return BadRequest("Lỗi khi cập nhật lô hàng.");

            var dto = new LoHangResponseDto
            {
                Id = entity.Id,
                SanPhamId = entity.SanPhamId,
                MaLo = entity.MaLo,
                NgaySanXuat = entity.NgaySanXuat,
                HanSuDung = entity.HanSuDung,
                SoLuong = entity.SoLuong,
                TieuChuanApDung = entity.TieuChuanApDung,
                KetQuaKiemNghiem = entity.KetQuaKiemNghiem,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return Ok(dto);
        }

        // DELETE: api/LoHangs/{id}  (Xoá mềm lô hàng)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _loHangRepo.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy lô hàng.");

            return NoContent();
        }
    }
}
