using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguoiTieuDungAuthController : ControllerBase
    {
        private readonly INguoiTieuDungRepository _repository;

        public NguoiTieuDungAuthController(INguoiTieuDungRepository repository)
        {
            _repository = repository;
        }

        // POST: api/NguoiTieuDungAuth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto<NguoiTieuDungResponseDto>>> Register(
            [FromBody] RegisterNguoiTieuDungRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto<NguoiTieuDungResponseDto>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ."
                });
            }

            if (request.MatKhau != request.XacNhanMatKhau)
            {
                return BadRequest(new AuthResponseDto<NguoiTieuDungResponseDto>
                {
                    Success = false,
                    Message = "Mật khẩu xác nhận không khớp."
                });
            }

            if (await _repository.EmailExistsAsync(request.Email))
            {
                return Conflict(new AuthResponseDto<NguoiTieuDungResponseDto>
                {
                    Success = false,
                    Message = "Email đã được đăng ký."
                });
            }

            var now = DateTime.UtcNow;

            var entity = new NguoiTieuDung
            {
                Id = Guid.NewGuid(),
                HoTen = request.HoTen,
                Email = request.Email,
                DienThoai = request.DienThoai,
                MatKhauHash = PasswordHasher.HashPassword(request.MatKhau),
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            var created = await _repository.CreateAsync(entity);

            var userDto = new NguoiTieuDungResponseDto
            {
                Id = created.Id,
                HoTen = created.HoTen,
                Email = created.Email!,
                DienThoai = created.DienThoai,
                CreatedAt = created.CreatedAt
            };

            return Ok(new AuthResponseDto<NguoiTieuDungResponseDto>
            {
                Success = true,
                Message = "Đăng ký thành công.",
                User = userDto,
                Token = null // sau này gắn JWT
            });
        }

        // POST: api/NguoiTieuDungAuth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto<NguoiTieuDungResponseDto>>> Login(
            [FromBody] LoginNguoiTieuDungRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDto<NguoiTieuDungResponseDto>
                {
                    Success = false,
                    Message = "Dữ liệu không hợp lệ."
                });
            }

            var user = await _repository.GetByEmailAsync(request.Email);

            if (user == null || !PasswordHasher.VerifyPassword(request.MatKhau, user.MatKhauHash))
            {
                return Unauthorized(new AuthResponseDto<NguoiTieuDungResponseDto>
                {
                    Success = false,
                    Message = "Email hoặc mật khẩu không đúng."
                });
            }

            var userDto = new NguoiTieuDungResponseDto
            {
                Id = user.Id,
                HoTen = user.HoTen,
                Email = user.Email!,
                DienThoai = user.DienThoai,
                CreatedAt = user.CreatedAt
            };

            // TODO: sinh JWT token nếu cần
            return Ok(new AuthResponseDto<NguoiTieuDungResponseDto>
            {
                Success = true,
                Message = "Đăng nhập thành công.",
                User = userDto,
                Token = null
            });
        }
    }
}
