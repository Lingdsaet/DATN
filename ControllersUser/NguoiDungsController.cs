using DATN.Model;
using DATN.ReponseDto;
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

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly INguoiDungRepository _nguoiDungRepo;
        private readonly IVaiTroRepository _vaiTroRepo;
        private readonly INguoiDungVaiTroRepository _nguoiDungVaiTroRepo;

        public AuthController(
            INguoiDungRepository nguoiDungRepo,
            IVaiTroRepository vaiTroRepo,
            INguoiDungVaiTroRepository nguoiDungVaiTroRepo)
        {
            _nguoiDungRepo = nguoiDungRepo;
            _vaiTroRepo = vaiTroRepo;
            _nguoiDungVaiTroRepo = nguoiDungVaiTroRepo;
        }

        [HttpPost("register-consumer")]
        public async Task<ActionResult> RegisterConsumer(
            [FromBody] RegisterNguoiTieuDungRequest request)
        {
            // 1. Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(request.Email) &&
                string.IsNullOrWhiteSpace(request.DienThoai))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Cần cung cấp ít nhất Email hoặc Số điện thoại."
                });
            }

            if (string.IsNullOrWhiteSpace(request.MatKhau) ||
                request.MatKhau != request.XacNhanMatKhau)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Mật khẩu và xác nhận mật khẩu không khớp."
                });
            }

            // 2. Check trùng Email / SĐT
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                if (await _nguoiDungRepo.EmailExistsAsync(request.Email))
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "Email đã được sử dụng."
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(request.DienThoai))
            {
                if (await _nguoiDungRepo.PhoneExistsAsync(request.DienThoai))
                {
                    return Conflict(new
                    {
                        Success = false,
                        Message = "Số điện thoại đã được sử dụng."
                    });
                }
            }

            // 3. Lấy vai trò NGUOI_TIEU_DUNG
            var vaiTroConsumer = await _vaiTroRepo.GetByMaAsync("NGUOI_TIEU_DUNG");
            if (vaiTroConsumer == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Success = false,
                    Message = "Chưa cấu hình vai trò NGUOI_TIEU_DUNG."
                });
            }

            // 4. Tạo user mới (DoanhNghiepId = null)
            var user = new NguoiDung
            {
                Id = Guid.NewGuid(),
                DoanhNghiepId = null,
                HoTen = request.HoTen,
                Email = request.Email,
                DienThoai = request.DienThoai,
                MatKhauHash = PasswordHasher.HashPassword(request.MatKhau),
                KichHoat = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            // 5. Mapping vai trò
            var mapping = new NguoiDungVaiTro
            {
                NguoiDungId = user.Id,
                VaiTroId = vaiTroConsumer.Id   // TINYINT
            };

            await _nguoiDungRepo.AddAsync(user);
            await _nguoiDungVaiTroRepo.AddAsync(mapping);
            await _nguoiDungRepo.SaveChangesAsync(); // Lưu tất cả

            // 6. Trả về DTO
            var response = new NguoiDungResponseDto
            {
                Id = user.Id,
                HoTen = user.HoTen,
                Email = user.Email,
                DienThoai = user.DienThoai
            };

            return Ok(new
            {
                Success = true,
                Message = "Đăng ký người dùng thành công.",
                Data = response
            });
        }

        [HttpPut("doi_thong_tin/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] NguoiDungUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var ok = await _nguoiDungRepo.UpdateThongTinAsync(id, dto);
            if (!ok)
                return NotFound("Không tìm thấy người dùng.");

            return Ok("Cập nhật thông tin thành công.");
        }

        [HttpPut("doi-mat-khau/{id}")]
        public async Task<IActionResult> DoiMatKhau(
        Guid id,
        [FromBody] DoiMatKhauDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.MatKhauCu) ||
                string.IsNullOrWhiteSpace(dto.MatKhauMoi))
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }

            try
            {
                var ok = await _nguoiDungRepo.DoiMatKhauAsync(id, dto);
                if (!ok)
                    return NotFound("Không tìm thấy người dùng.");

                return Ok("Đổi mật khẩu thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
