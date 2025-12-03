using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace DATN1.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDoanhNghiepController : ControllerBase
    {
        private readonly INguoiDungRepository _nguoiDungRepo;
        private readonly IDoanhNghiepRepository _doanhNghiepRepo;
        private readonly INguoiDungVaiTroRepository _nguoiDungVaiTroRepo;
        private readonly IVaiTroRepository _vaiTroRepo;

        public AdminDoanhNghiepController(
        INguoiDungRepository nguoiDungRepo,
        IDoanhNghiepRepository doanhNghiepRepo,
        INguoiDungVaiTroRepository nguoiDungVaiTroRepo,
        IVaiTroRepository vaiTroRepo)
        {
            _nguoiDungRepo = nguoiDungRepo;
            _doanhNghiepRepo = doanhNghiepRepo;
            _nguoiDungVaiTroRepo = nguoiDungVaiTroRepo;
            _vaiTroRepo = vaiTroRepo;
        }

        // GET: api/DoanhNghieps?trangThai=ACTIVE
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoanhNghiepResponseDto>>> GetAll(
            [FromQuery] string? trangThai)
        {
            var list = await _doanhNghiepRepo.GetAllAsync(trangThai);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // GET: api/DoanhNghieps/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DoanhNghiepResponseDto>> GetById(Guid id)
        {
            var entity = await _doanhNghiepRepo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy doanh nghiệp.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        public async Task<IActionResult> GanDoanhNghiepChoNguoiDung(
        [FromBody] GanDoanhNghiepChoNguoiDungRequest request)
        {
            // 1. Tìm user
            var user = await _nguoiDungRepo.GetByIdAsync(request.NguoiDungId);
            if (user == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng."
                });
            }

            // 2. Tạo DoanhNghiep mới
            var dn = new DoanhNghiep
            {
                Id = Guid.NewGuid(),
                Ten = request.TenDoanhNghiep,
                MaSoThue = request.MaSoThue,
                DiaChi = request.DiaChi,
                DienThoai = request.DienThoai,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _doanhNghiepRepo.AddAsync(dn);

            // 3. Gán DoanhNghiepId cho user
            user.DoanhNghiepId = dn.Id;
            user.UpdatedAt = DateTime.UtcNow;
            await _nguoiDungRepo.UpdateAsync(user);

            // 4. Lấy vai trò DOANH_NGHIEP
            var vaiTroDoanhNghiep = await _vaiTroRepo.GetByMaAsync("DOANH_NGHIEP");
            if (vaiTroDoanhNghiep == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Chưa cấu hình vai trò DOANH_NGHIEP trong bảng VaiTro."
                });
            }

            // 5. Xoá hết role cũ của user, thêm role DOANH_NGHIEP
            await _nguoiDungVaiTroRepo.RemoveAllRolesOfUserAsync(user.Id);

            var mapping = new NguoiDungVaiTro
            {
                NguoiDungId = user.Id,
                VaiTroId = vaiTroDoanhNghiep.Id   // tinyint/byte
            };
            await _nguoiDungVaiTroRepo.AddAsync(mapping);

            // 6. Lưu thay đổi
            await _nguoiDungVaiTroRepo.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Gán doanh nghiệp và cập nhật vai trò DOANH_NGHIEP thành công.",
                Data = new
                {
                    UserId = user.Id,
                    DoanhNghiepId = dn.Id
                }
            });
        }
        // POST: api/DoanhNghieps  (Thêm doanh nghiệp)
        //[HttpPost]
        //public async Task<ActionResult<DoanhNghiepResponseDto>> Create(
        //    [FromBody] DoanhNghiepCreateRequestDto request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var now = DateTime.UtcNow;

        //    var entity = new DoanhNghiep
        //    {
        //        Id = Guid.NewGuid(),
        //        Ten = request.Ten,
        //        MaSoThue = request.MaSoThue,
        //        Email = request.Email,
        //        DienThoai = request.DienThoai,
        //        DiaChi = request.DiaChi,
        //        TrangThai = request.TrangThai,
        //        CreatedAt = now,
        //        UpdatedAt = now,
        //        XoaMem = false
        //    };

        //    entity = await _doanhNghiepRepo.CreateAsync(entity);
        //    var dto = MapToDto(entity);

        //    return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        //}

        // PUT: api/DoanhNghieps/{id}  (Sửa doanh nghiệp)
        [HttpPut("{id}")]
        public async Task<ActionResult<DoanhNghiepResponseDto>> Update(
            Guid id,
            [FromBody] DoanhNghiepUpdateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _doanhNghiepRepo.GetByIdAsync(id);
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

            entity = await _doanhNghiepRepo.UpdateAsync(entity);
            var dto = MapToDto(entity);

            return Ok(dto);
        }

        // DELETE: api/DoanhNghieps/{id}  (Xoá mềm)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _doanhNghiepRepo.SoftDeleteAsync(id);
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
