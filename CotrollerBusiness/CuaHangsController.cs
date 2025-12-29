using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.CotrollerBusiness
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuaHangsController : ControllerBase
    {
        private readonly ICuaHangRepository _cuaHangRepo;
        private readonly QR_DATNContext _context;

        public CuaHangsController(ICuaHangRepository cuaHangRepo, QR_DATNContext context)
        {
            _cuaHangRepo = cuaHangRepo;
            _context = context;
        }

        // GET: api/CuaHangs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CuaHangResponseDto>> GetById(Guid id)
        {
            var entity = await _cuaHangRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy cửa hàng.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        // GET: api/CuaHangs/doanh-nghiep/{doanhNghiepId}
        [HttpGet("doanh-nghiep/{doanhNghiepId}")]
        public async Task<ActionResult<IEnumerable<CuaHangResponseDto>>> GetByDoanhNghiep(Guid doanhNghiepId)
        {
            var list = await _cuaHangRepo.GetByDoanhNghiepIdAsync(doanhNghiepId);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // POST: api/CuaHangs  (Thêm cửa hàng)
        [HttpPost("themCuaHang")]
        public async Task<ActionResult<CuaHangResponseDto>> Create([FromBody] CuaHangCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra doanh nghiệp tồn tại
            var dnExists = await _context.DoanhNghieps
                .AnyAsync(x => x.Id == request.DoanhNghiepId && !x.XoaMem);

            if (!dnExists)
                return BadRequest("Doanh nghiệp không tồn tại.");

            // Nếu có DiaDiemId thì kiểm tra luôn
            if (request.DiaDiemId.HasValue)
            {
                var ddExists = await _context.DiaDiems
                    .AnyAsync(x => x.Id == request.DiaDiemId.Value && !x.XoaMem);
                if (!ddExists)
                    return BadRequest("Địa điểm không tồn tại.");
            }

            var now = DateTime.UtcNow;

            var entity = new CuaHang
            {
                Id = Guid.NewGuid(),
                DoanhNghiepId = request.DoanhNghiepId,
                Ten = request.Ten,
                DiaDiemId = request.DiaDiemId,
                LienHe = request.LienHe,
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            entity = await _cuaHangRepo.CreateAsync(entity);
            var dto = MapToDto(entity);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/CuaHangs/{id}  (Sửa cửa hàng)
        [HttpPut("sua/{id}")]
        public async Task<ActionResult<CuaHangResponseDto>> Update(
            Guid id,
            [FromBody] CuaHangUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _cuaHangRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy cửa hàng.");

            if (!string.IsNullOrWhiteSpace(request.Ten))
                entity.Ten = request.Ten;

            if (request.DiaDiemId.HasValue)
            {
                // kiểm tra DiaDiem tồn tại
                var ddExists = await _context.DiaDiems
                    .AnyAsync(x => x.Id == request.DiaDiemId.Value && !x.XoaMem);
                if (!ddExists)
                    return BadRequest("Địa điểm không tồn tại.");

                entity.DiaDiemId = request.DiaDiemId;
            }

            if (request.LienHe != null)
                entity.LienHe = request.LienHe;

            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _cuaHangRepo.UpdateAsync(entity);
            var dto = MapToDto(entity);

            return Ok(dto);
        }

        // DELETE: api/CuaHangs/{id}  (Xoá mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _cuaHangRepo.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy cửa hàng.");

            return NoContent();
        }

        // Helper map entity -> DTO
        private static CuaHangResponseDto MapToDto(CuaHang entity)
        {
            return new CuaHangResponseDto
            {
                Id = entity.Id,
                DoanhNghiepId = entity.DoanhNghiepId,
                Ten = entity.Ten,
                DiaDiemId = entity.DiaDiemId,
                LienHe = entity.LienHe,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
