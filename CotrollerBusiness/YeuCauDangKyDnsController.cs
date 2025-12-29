using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN.CotrollerBusiness
{
    [ApiController]
    [Route("api/[controller]")]
    public class YeuCauDangKyDnController : ControllerBase
    {
        private readonly IYeuCauDangKyDnRepository _repo;

        public YeuCauDangKyDnController(IYeuCauDangKyDnRepository repo)
        {
            _repo = repo;
        }

        // 1) Doanh nghiệp gửi yêu cầu đăng ký
        // POST: api/YeuCauDangKyDn
        [HttpPost]
        public async Task<ActionResult<YeuCauDangKyDnResponseDto>> Create(
            [FromBody] YeuCauDangKyDnCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var now = DateTime.UtcNow;

            var entity = new YeuCauDangKyDn
            {
                Id = Guid.NewGuid(),
                TenDoanhNghiep = request.TenDoanhNghiep,
                MaSoThue = request.MaSoThue,
                Email = request.Email,
                DienThoai = request.DienThoai,
                DiaChi = request.DiaChi,
                TrangThai = "PENDING",
                GhiChu = null,
                CreatedAt = now,
                UpdatedAt = now
            };

            entity = await _repo.CreateAsync(entity);
            var dto = MapToDto(entity);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // 2) Admin xem danh sách yêu cầu (lọc theo trạng thái)
        // GET: api/YeuCauDangKyDn?trangThai=PENDING
        [HttpGet("Admin")]
        public async Task<ActionResult<IEnumerable<YeuCauDangKyDnResponseDto>>> GetAll(
            [FromQuery] string? trangThai)
        {
            var list = await _repo.GetAllAsync(trangThai);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // 3) Admin xem chi tiết 1 yêu cầu
        // GET: api/YeuCauDangKyDn/{id}
        [HttpGet("Admin/{id}")]
        public async Task<ActionResult<YeuCauDangKyDnResponseDto>> GetById(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy yêu cầu.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        // 4) Admin cập nhật trạng thái (duyệt / từ chối)
        // PUT: api/YeuCauDangKyDn/{id}
        [HttpPut("Admin/{id}")]
        public async Task<ActionResult<YeuCauDangKyDnResponseDto>> UpdateStatus(
            Guid id,
            [FromBody] YeuCauDangKyDnUpdateStatusRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy yêu cầu.");

            entity.TrangThai = request.TrangThai;
            entity.GhiChu = request.GhiChu;
            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _repo.UpdateAsync(entity);
            var dto = MapToDto(entity);

            return Ok(dto);
        }

        // Helper map
        private static YeuCauDangKyDnResponseDto MapToDto(YeuCauDangKyDn x)
        {
            return new YeuCauDangKyDnResponseDto
            {
                Id = x.Id,
                TenDoanhNghiep = x.TenDoanhNghiep,
                MaSoThue = x.MaSoThue,
                Email = x.Email,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                TrangThai = x.TrangThai,
                GhiChu = x.GhiChu,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            };
        }
    }
}
