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
    public class SanPhamsController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepo;
        private readonly QR_DATNContext _context;

        public SanPhamsController(ISanPhamRepository sanPhamRepo, QR_DATNContext context)
        {
            _sanPhamRepo = sanPhamRepo;
            _context = context;
        }

        // GET: api/SanPhams/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SanPhamResponseDto>> GetById(Guid id)
        {
            var entity = await _sanPhamRepo.GetByIdAsync(id);
            if (entity == null) return NotFound("Không tìm thấy sản phẩm.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        // GET: api/SanPhams/doanh-nghiep/{doanhNghiepId}
        [HttpGet("doanh-nghiep/{doanhNghiepId}")]
        public async Task<ActionResult<IEnumerable<SanPhamResponseDto>>> GetByDoanhNghiep(Guid doanhNghiepId)
        {
            var list = await _sanPhamRepo.GetByDoanhNghiepIdAsync(doanhNghiepId);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // POST: api/SanPhams   (thêm sản phẩm)
        [HttpPost]
        public async Task<ActionResult<SanPhamResponseDto>> Create([FromBody] SanPhamCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra doanh nghiệp tồn tại
            var dnExists = await _context.DoanhNghieps
                .AnyAsync(x => x.Id == request.DoanhNghiepId && !x.XoaMem);

            if (!dnExists)
                return BadRequest("Doanh nghiệp không tồn tại.");

            var now = DateTime.UtcNow;

            var entity = new SanPham
            {
                Id = Guid.NewGuid(),
                DoanhNghiepId = request.DoanhNghiepId,
                Ten = request.Ten,
                MaSanPham = request.MaSanPham,
                MoTa = request.MoTa,
                HinhAnhUrl = request.HinhAnhUrl,
                TieuChuanApDung = request.TieuChuanApDung,
                TrangThai = request.TrangThai,
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            entity = await _sanPhamRepo.CreateAsync(entity);
            var dto = MapToDto(entity);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/SanPhams/{id}   (sửa sản phẩm)
        [HttpPut("{id}")]
        public async Task<ActionResult<SanPhamResponseDto>> Update(
            Guid id,
            [FromBody] SanPhamUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _sanPhamRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy sản phẩm.");

            if (!string.IsNullOrWhiteSpace(request.Ten))
                entity.Ten = request.Ten;

            if (request.MaSanPham != null)
                entity.MaSanPham = request.MaSanPham;

            if (request.MoTa != null)
                entity.MoTa = request.MoTa;

            if (request.HinhAnhUrl != null)
                entity.HinhAnhUrl = request.HinhAnhUrl;

            if (request.TieuChuanApDung != null)
                entity.TieuChuanApDung = request.TieuChuanApDung;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                entity.TrangThai = request.TrangThai;

            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _sanPhamRepo.UpdateAsync(entity);
            var dto = MapToDto(entity);

            return Ok(dto);
        }

        // DELETE: api/SanPhams/{id}   (xoá mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _sanPhamRepo.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy sản phẩm.");

            return NoContent();
        }

        // Helper map entity -> DTO
        private static SanPhamResponseDto MapToDto(SanPham entity)
        {
            return new SanPhamResponseDto
            {
                Id = entity.Id,
                DoanhNghiepId = entity.DoanhNghiepId,
                Ten = entity.Ten,
                MaSanPham = entity.MaSanPham,
                MoTa = entity.MoTa,
                HinhAnhUrl = entity.HinhAnhUrl,
                TieuChuanApDung = entity.TieuChuanApDung,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
