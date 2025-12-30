using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN.ControllersAdmin
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungRepository _repo;

        public NguoiDungController(INguoiDungRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repo.GetAllAsync();

            var result = users.Select(x => new NguoiDungAdminResponseDto
            {
                Id = x.Id,
                HoTen = x.HoTen,
                Email = x.Email,
                DienThoai = x.DienThoai,
                KichHoat = x.KichHoat,
                VaiTros = x.NguoiDungVaiTros.Select(v => v.VaiTro.Ten).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(new NguoiDungAdminResponseDto
            {
                Id = user.Id,
                HoTen = user.HoTen,
                Email = user.Email,
                DienThoai = user.DienThoai,
                KichHoat = user.KichHoat,
                VaiTros = user.NguoiDungVaiTros.Select(v => v.VaiTro.Ten).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(NguoiDungCreateRequest request)
        {
            if (await _repo.EmailExistsAsync(request.Email))
                return BadRequest("Email đã tồn tại");

            var entity = new NguoiDung
            {
                Id = Guid.NewGuid(),
                HoTen = request.HoTen,
                Email = request.Email,
                DienThoai = request.DienThoai,
                DoanhNghiepId = request.DoanhNghiepId,
                KichHoat = request.KichHoat,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            await _repo.AddAsync(entity);
            return Ok("Tạo người dùng thành công");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, NguoiDungCreateRequest request)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            user.HoTen = request.HoTen;
            user.DienThoai = request.DienThoai;
            user.KichHoat = request.KichHoat;
            user.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(user);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.SoftDeleteAsync(id);
            return Ok("Đã xóa người dùng");
        }

        [HttpPost("GanVaiTro")]
        public async Task<IActionResult> GanVaiTro(GanVaiTroRequest request)
        {
            await _repo.GanVaiTroAsync(request.NguoiDungId, request.VaiTroIds);
            return Ok("Gán vai trò thành công");
        }
    }


}
