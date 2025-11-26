using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN1.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoanhNghiepsController : ControllerBase
    {
        private readonly IDoanhNghiepRepository _repo;

        public DoanhNghiepsController(IDoanhNghiepRepository repo)
        {
            _repo = repo;
        }

        // GET: api/DoanhNghieps?trangThai=ACTIVE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoanhNghiepResponseDto>>> GetAll(
            [FromQuery] string? trangThai)
        {
            var list = await _repo.GetAllAsync(trangThai);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // GET: api/DoanhNghieps/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DoanhNghiepResponseDto>> GetById(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy doanh nghiệp.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        // POST: api/DoanhNghieps  (Thêm doanh nghiệp)
        [HttpPost]
        public async Task<ActionResult<DoanhNghiepResponseDto>> Create(
            [FromBody] DoanhNghiepCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var now = DateTime.UtcNow;

            var entity = new DoanhNghiep
            {
                Id = Guid.NewGuid(),
                Ten = request.Ten,
                MaSoThue = request.MaSoThue,
                Email = request.Email,
                DienThoai = request.DienThoai,
                DiaChi = request.DiaChi,
                TrangThai = request.TrangThai,
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            entity = await _repo.CreateAsync(entity);
            var dto = MapToDto(entity);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/DoanhNghieps/{id}  (Sửa doanh nghiệp)
        [HttpPut("{id}")]
        public async Task<ActionResult<DoanhNghiepResponseDto>> Update(
            Guid id,
            [FromBody] DoanhNghiepUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy doanh nghiệp.");

            if (!string.IsNullOrWhiteSpace(request.Ten))
                entity.Ten = request.Ten;

            if (request.MaSoThue != null)
                entity.MaSoThue = request.MaSoThue;

            if (request.Email != null)
                entity.Email = request.Email;

            if (request.DienThoai != null)
                entity.DienThoai = request.DienThoai;

            if (request.DiaChi != null)
                entity.DiaChi = request.DiaChi;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                entity.TrangThai = request.TrangThai;

            entity.UpdatedAt = DateTime.UtcNow;

            entity = await _repo.UpdateAsync(entity);
            var dto = MapToDto(entity);

            return Ok(dto);
        }

        // DELETE: api/DoanhNghieps/{id}  (Xoá mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repo.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy doanh nghiệp.");

            return NoContent();
        }

        // Helper map entity -> DTO
        private static DoanhNghiepResponseDto MapToDto(DoanhNghiep x)
        {
            return new DoanhNghiepResponseDto
            {
                Id = x.Id,
                Ten = x.Ten,
                MaSoThue = x.MaSoThue,
                Email = x.Email,
                DienThoai = x.DienThoai,
                DiaChi = x.DiaChi,
                TrangThai = x.TrangThai,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            };
        }
    }
}
