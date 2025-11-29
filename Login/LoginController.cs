using DATN.Model;
using DATN.Repository;
using DATN.RequestDto;
using DATN.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly INguoiDungRepository _nguoiDungRepo;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(
            INguoiDungRepository nguoiDungRepo,
            IJwtTokenService jwtTokenService)
        {
            _nguoiDungRepo = nguoiDungRepo;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto<LoginResultDto>>> Login(
            [FromBody] LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailOrPhone) ||
                string.IsNullOrWhiteSpace(request.MatKhau))
            {
                return BadRequest(new AuthResponseDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Vui lòng nhập đủ tài khoản và mật khẩu."
                });
            }

            var user = await _nguoiDungRepo.GetByEmailOrPhoneAsync(request.EmailOrPhone);

            if (user == null ||
                !PasswordHasher.VerifyPassword(request.MatKhau, user.MatKhauHash))
            {
                return Unauthorized(new AuthResponseDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Tài khoản hoặc mật khẩu không đúng."
                });
            }

            if (user.XoaMem)
            {
                return Unauthorized(new AuthResponseDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Tài khoản đã bị khóa hoặc xóa."
                });
            }

            if (!user.KichHoat)
            {
                return Unauthorized(new AuthResponseDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Tài khoản chưa được kích hoạt."
                });
            }

            // Lấy role
            var roles = await _nguoiDungRepo.GetRoleCodesAsync(user.Id);

            // Map user DTO
            var userDto = new LoginUserDto
            {
                Id = user.Id,
                HoTen = user.HoTen,
                Email = user.Email,
                DienThoai = user.DienThoai,
                Roles = roles
            };

            // Tạo token
            var token = _jwtTokenService.CreateToken(user, roles, out var expiresAt);

            var result = new LoginResultDto
            {
                User = userDto,
                Token = token,
                ExpiresAt = expiresAt
            };

            return Ok(new AuthResponseDto<LoginResultDto>
            {
                Success = true,
                Message = "Đăng nhập thành công.",
                Data = result
            });
        }
    }

}
