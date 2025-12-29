using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class SanPhamsController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepo;
        private readonly QR_DATNContext _context;
        private readonly IFirebaseService _firebaseService;
        private readonly IMaQrSanPhamRepository _maQrRepo;

        public SanPhamsController(ISanPhamRepository sanPhamRepo, QR_DATNContext context, IFirebaseService firebaseService, IMaQrSanPhamRepository maQrRepo)
        {
            _sanPhamRepo = sanPhamRepo;
            _context = context;
            _firebaseService = firebaseService;
            _maQrRepo = maQrRepo;
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
        [HttpPost]
        public async Task<ActionResult<SanPhamResponseDto>> Create([FromForm] CreateSanPhamDto dto)
        {
            try
            {
                // Validate model
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ",
                        ModelState.Values.SelectMany(v => v.Errors)
                                         .Select(e => e.ErrorMessage));
                    return BadRequest("Dữ liệu không hợp lệ: " + errors);
                }

                // Kiểm tra Doanh nghiệp tồn tại
                var dnExists = await _context.DoanhNghieps
                    .AnyAsync(x => x.Id == dto.DoanhNghiepId && !x.XoaMem);

                if (!dnExists)
                    return BadRequest("Doanh nghiệp không tồn tại.");

                var existed = await _context.SanPhams.AnyAsync(x =>
                    x.DoanhNghiepId == dto.DoanhNghiepId &&
                    x.Ten == dto.Ten &&
                    !x.XoaMem);

                if (existed)
                    return BadRequest("Sản phẩm đã tồn tại trong doanh nghiệp.");
                // Kiểm tra Loại sản phẩm (nếu có)
                if (dto.LoaiSanPhamId.HasValue)
                {
                    var loaiExists = await _context.LoaiSanPhams
                        .AnyAsync(x => x.Id == dto.LoaiSanPhamId && !x.XoaMem);

                    if (!loaiExists)
                        return BadRequest("Loại sản phẩm không tồn tại.");
                }

                // Validate file ảnh
                if (dto.HinhAnh != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(dto.HinhAnh.FileName).ToLowerInvariant();
                    
                    if (!allowedExtensions.Contains(fileExtension))
                        return BadRequest("Chỉ chấp nhận các định dạng: jpg, jpeg, png, gif");

                    const long maxFileSize = 5 * 1024 * 1024; // 5MB
                    if (dto.HinhAnh.Length > maxFileSize)
                        return BadRequest("Kích thước ảnh không được vượt quá 5MB");

                    if (dto.HinhAnh.Length < 100)
                        return BadRequest("Tệp ảnh quá nhỏ");
                }

                // Validate nghiệp vụ
                if (dto.HanSuDung.HasValue && dto.NgaySanXuat.HasValue &&
                    dto.HanSuDung < dto.NgaySanXuat)
                {
                    return BadRequest("Hạn sử dụng phải >= ngày sản xuất.");
                }

                if (dto.Gia.HasValue && dto.Gia < 0)
                {
                    return BadRequest("Giá sản phẩm không hợp lệ.");
                }

                //  Upload ảnh Firebase
                string? imageUrl = null;
                if (dto.HinhAnh != null && dto.HinhAnh.Length > 0)
                {
                    imageUrl = await _firebaseService
                        .UploadImageToFirebase(dto.HinhAnh);
                }

                //  Tạo entity SanPham
                var sanPham = new SanPham
                {
                    Id = Guid.NewGuid(),
                    DoanhNghiepId = dto.DoanhNghiepId,
                    LoaiSanPhamId = dto.LoaiSanPhamId,

                    Ten = dto.Ten,
                    MaSanPham = dto.MaSanPham,
                    MoTa = dto.MoTa,
                    TieuChuanApDung = dto.TieuChuanApDung,

                    Gia = dto.Gia,
                    SoLuong = dto.SoLuong,
                    DonViTinh = dto.DonViTinh,

                    NgaySanXuat = dto.NgaySanXuat,
                    HanSuDung = dto.HanSuDung,
                    NoiSanXuat = dto.NoiSanXuat,

                    HinhAnhUrl = imageUrl,

                    TrangThai = "PENDING",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    XoaMem = false
                };

                //  Lưu DB
                await _sanPhamRepo.AddAsync(sanPham);
                await _context.SaveChangesAsync();

                // Tạo QR code với xử lý lỗi
                MaQrSanPham? qrEntity = null;
                try
                {
                    qrEntity = await _maQrRepo.CreateForSanPhamAsync(sanPham.Id, "QR sản phẩm");
                }
                catch (Exception qrEx)
                {
                    // Log lỗi nhưng không fail toàn bộ request
                    Console.WriteLine($"Lỗi tạo QR code: {qrEx.Message}");
                }

                // Trả về DTO
                var response = new SanPhamResponseDto
                {
                    Id = sanPham.Id,
                    DoanhNghiepId = sanPham.DoanhNghiepId,
                    LoaiSanPhamId = sanPham.LoaiSanPhamId,
                    Ten = sanPham.Ten,
                    MaSanPham = sanPham.MaSanPham,
                    MoTa = sanPham.MoTa,
                    Gia = sanPham.Gia,
                    SoLuong = sanPham.SoLuong,
                    DonViTinh = sanPham.DonViTinh,
                    NgaySanXuat = sanPham.NgaySanXuat,
                    HanSuDung = sanPham.HanSuDung,
                    NoiSanXuat = sanPham.NoiSanXuat,
                    HinhAnhUrl = sanPham.HinhAnhUrl,
                    QrCode = qrEntity?.MaQr,
                    TrangThai = sanPham.TrangThai,
                    CreatedAt = sanPham.CreatedAt,
                    UpdatedAt = sanPham.UpdatedAt
                };

                return CreatedAtAction(nameof(GetById),
                    new { id = sanPham.Id }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Lỗi server",
                    Error = ex.Message
                });
            }
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
                Gia = entity.Gia,
                SoLuong = entity.SoLuong,
                DonViTinh = entity.DonViTinh,
                NgaySanXuat = entity.NgaySanXuat,
                HanSuDung = entity.HanSuDung,
                NoiSanXuat = entity.NoiSanXuat,
                HinhAnhUrl = entity.HinhAnhUrl,
                QrCode = entity.Qrcode,
                TieuChuanApDung = entity.TieuChuanApDung,
                TrangThai = entity.TrangThai,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
