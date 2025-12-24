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

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepository;

        public SanPhamController(ISanPhamRepository sanPhamRepository)
        {
            _sanPhamRepository = sanPhamRepository;
        }


        [HttpGet]
        public async Task<ActionResult<object>> GetAll()
        {
            var list = await _sanPhamRepository.GetAllAsync();

            var result = list.Select(sp => new SanPhamResponseDto
            {
                Id = sp.Id,
                DoanhNghiepId = sp.DoanhNghiepId,
                MaSanPham = sp.MaSanPham,
                Ten = sp.Ten,
                MoTa = sp.MoTa,
                HinhAnhUrl = sp.HinhAnhUrl
            }).ToList();

            return Ok(new
            {
                Success = true,
                Message = "Lấy danh sách sản phẩm thành công.",
                Data = result
            });
        }

        // GET: api/SanPhams/doanh-nghiep/{enterpriseId}
        [HttpGet("doanh-nghiep/{enterpriseId}")]
        public async Task<ActionResult<object>> GetByEnterprise(Guid enterpriseId)
        {
            var list = await _sanPhamRepository.GetAllAsync();
            var filtered = list.Where(x => x.DoanhNghiepId == enterpriseId).ToList();

            var result = filtered.Select(sp => new SanPhamResponseDto
            {
                Id = sp.Id,
                DoanhNghiepId = sp.DoanhNghiepId,
                MaSanPham = sp.MaSanPham,
                Ten = sp.Ten,
                MoTa = sp.MoTa,
                HinhAnhUrl = sp.HinhAnhUrl
            }).ToList();

            return Ok(result);
        }

        // POST: api/SanPhams
        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] CreateSanPhamDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Ten) || string.IsNullOrWhiteSpace(request.MaSanPham))
                return BadRequest("Tên và mã sản phẩm là bắt buộc");

            var sanPham = new SanPham
            {
                Id = Guid.NewGuid(),
                DoanhNghiepId = request.DoanhNghiepId,
                MaSanPham = request.MaSanPham,
                Ten = request.Ten,
                MoTa = request.MoTa,
                HinhAnhUrl = null, // Sẽ được set khi upload ảnh
                TieuChuanApDung = request.TieuChuanApDung,
                TrangThai = "ACTIVE", // Mặc định ACTIVE
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            var created = await _sanPhamRepository.AddAsync(sanPham);
            return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
        }

        // PUT: api/SanPhams/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(Guid id, [FromBody] SanPhamUpdateRequestDto request)
        {
            var existing = await _sanPhamRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound("Không tìm thấy sản phẩm");

            if (!string.IsNullOrWhiteSpace(request.MaSanPham))
                existing.MaSanPham = request.MaSanPham;

            if (!string.IsNullOrWhiteSpace(request.Ten))
                existing.Ten = request.Ten;

            if (request.MoTa != null)
                existing.MoTa = request.MoTa;

            if (!string.IsNullOrWhiteSpace(request.HinhAnhUrl))
                existing.HinhAnhUrl = request.HinhAnhUrl;

            if (!string.IsNullOrWhiteSpace(request.TieuChuanApDung))
                existing.TieuChuanApDung = request.TieuChuanApDung;

            if (!string.IsNullOrWhiteSpace(request.TrangThai))
                existing.TrangThai = request.TrangThai;

            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _sanPhamRepository.UpdateAsync(existing);
            return Ok(new { message = "Cập nhật thành công", data = updated });
        }

        // DELETE: api/SanPhams/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(Guid id)
        {
            var success = await _sanPhamRepository.SoftDeleteAsync(id);
            if (!success)
                return NotFound("Không tìm thấy sản phẩm");

            return Ok(new { message = "Xóa thành công" });
        }    }
}