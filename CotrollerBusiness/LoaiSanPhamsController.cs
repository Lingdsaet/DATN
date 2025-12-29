using DATN.Model;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN.CotrollerBusiness
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoaiSanPhamsController : ControllerBase
    {
        private readonly ILoaiSanPhamRepository _repo;

        public LoaiSanPhamsController(ILoaiSanPhamRepository repo)
        {
            _repo = repo;
        }

        // GET: api/LoaiSanPhams
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();

            var result = list.Select(x => new
            {
                x.Id,
                x.TenLoai,
                x.MaLoai,
                x.MoTa,
                x.TrangThai,
                x.CreatedAt,
                x.UpdatedAt
            });

            return Ok(result);
        }

        // GET: api/LoaiSanPhams/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy loại sản phẩm.");

            return Ok(entity);
        }

        // POST: api/LoaiSanPhams
        [HttpPost]
        public async Task<IActionResult> Create(LoaiSanPhamCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _repo.ExistsByMaLoaiAsync(request.MaLoai))
                return BadRequest("Mã loại sản phẩm đã tồn tại.");

            var entity = new LoaiSanPham
            {
                Id = Guid.NewGuid(),
                TenLoai = request.TenLoai,
                MaLoai = request.MaLoai,
                MoTa = request.MoTa,
                TrangThai = "ACTIVE",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            await _repo.AddAsync(entity);
            return Ok("Tạo loại sản phẩm thành công.");
        }

        // PUT: api/LoaiSanPhams/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, LoaiSanPhamUpdateRequestDto request)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy loại sản phẩm.");

            if (!string.IsNullOrWhiteSpace(request.TenLoai))
                entity.TenLoai = request.TenLoai;

            if (request.MoTa != null)
                entity.MoTa = request.MoTa;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                entity.TrangThai = request.TrangThai;

            entity.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            return Ok("Cập nhật loại sản phẩm thành công.");
        }

        // DELETE: api/LoaiSanPhams/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repo.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy loại sản phẩm.");

            return NoContent();
        }
    }
}
