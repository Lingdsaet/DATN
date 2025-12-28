using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using DATN.Utils;
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
        private readonly IFirebaseService _firebaseService;

        public SanPhamsController(ISanPhamRepository sanPhamRepo, QR_DATNContext context, IFirebaseService firebaseService)
        {
            _sanPhamRepo = sanPhamRepo;
            _context = context;
            _firebaseService = firebaseService;
        }

        // GET: api/SanPhams (tất cả sản phẩm)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPhamResponseDto>>> GetAll()
        {
            var list = await _context.SanPhams.Where(x => !x.XoaMem).ToListAsync();
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // GET: api/SanPhams/doanh-nghiep/{doanhNghiepId} 
        [HttpGet("doanh-nghiep/{doanhNghiepId}")]
        public async Task<ActionResult<IEnumerable<SanPhamResponseDto>>> GetByDoanhNghiep(Guid doanhNghiepId)
        {
            var list = await _sanPhamRepo.GetByDoanhNghiepIdAsync(doanhNghiepId);
            var result = list.Select(MapToDto);
            return Ok(result);
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

        // POST: api/SanPhams   (thêm sản phẩm)
        [HttpPost("ThemSanPham")]
        public async Task<ActionResult<SanPhamResponseDto>> Create([FromForm] CreateSanPhamDto dto)
        {
            // BẮT ĐẦU: TRY-CATCH DEBUG
            try
            {
                // 1. Validate Model
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage));
                    return BadRequest("Lỗi dữ liệu: " + errors);
                }

                // 2. Kiểm tra Doanh Nghiệp 
                var dnExists = await _context.DoanhNghieps
                    .AnyAsync(x => x.Id == dto.DoanhNghiepId && !x.XoaMem);

                if (!dnExists)
                    return BadRequest($"Doanh nghiệp ID {dto.DoanhNghiepId} không tồn tại.");

                // 3. Upload ảnh 
                string? imageUrl = null;
                if (dto.HinhAnh != null && dto.HinhAnh.Length > 0)
                {
                    imageUrl = await _firebaseService.UploadImageToFirebase(dto.HinhAnh);
                }

                // 4. Tạo Entity
                var sanPham = new SanPham
                {
                    Id = Guid.NewGuid(),
                    DoanhNghiepId = dto.DoanhNghiepId,
                    Ten = dto.Ten,
                    MaSanPham = dto.MaSanPham,
                    MoTa = dto.MoTa,
                    TieuChuanApDung = dto.TieuChuanApDung,
                    HinhAnhUrl = imageUrl,
                    TrangThai = "PENDING",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    XoaMem = false
                };

                // 5. Lưu DB
                await _sanPhamRepo.AddAsync(sanPham);

                // 6. Trả về
                var response = MapToDto(sanPham); // Đảm bảo hàm MapToDto không lỗi
                return CreatedAtAction(nameof(GetById), new { id = sanPham.Id }, response);
            }
            catch (Exception ex)
            {
                //  Trả về lỗi chi tiết để đọc
                return StatusCode(500, new
                {
                    Message = "Server gặp lỗi nội bộ",
                    Error = ex.Message,
                    StackTrace = ex.ToString()
                });
            }
        }



        // PUT: api/SanPhams/{id}   (sửa sản phẩm)
        [HttpPut("sua/{id}")]
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
