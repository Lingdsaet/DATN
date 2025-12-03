using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
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
    public class YeuCauDangKyDnController : ControllerBase
    {
        private readonly IYeuCauDangKyDnRepository _repository;

        public YeuCauDangKyDnController(IYeuCauDangKyDnRepository repository)
        {
            _repository = repository;
        }

        // [Authorize(Roles = "ADMIN")] // nếu chỉ admin được xem thì bật dòng này
        [HttpGet]
        public async Task<ActionResult<object>> GetAll()
        {
            var entities = await _repository.GetAllAsync();

            var data = entities.Select(x => new YeuCauDangKyDnResponseDto
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
            }).ToList();

            return Ok(new
            {
                Success = true,
                Message = "Lấy danh sách yêu cầu đăng ký doanh nghiệp thành công.",
                Data = data
            });
        }

        // DELETE: api/YeuCauDangKyDns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _repository.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy yêu cầu.");

            return NoContent();
        }
    }
}
