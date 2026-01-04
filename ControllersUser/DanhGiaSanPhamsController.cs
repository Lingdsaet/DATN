using DATN.Model;
using DATN.Model.DATN.Model;
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
    public class DanhGiaSanPhamController : ControllerBase
    {
        private readonly IDanhGiaSanPhamRepository _repo;
        private readonly QR_DATNContext _context;

        public DanhGiaSanPhamController(
            IDanhGiaSanPhamRepository repo,
            QR_DATNContext context)
        {
            _repo = repo;
            _context = context;
        }

        // POST: api/DanhGiaSanPham
        [HttpPost("danh gia")]
        public async Task<IActionResult> Create(DanhGiaRequestDto dto)
        {
            if (dto.SoSao < 1 || dto.SoSao > 5)
                return BadRequest("Số sao phải từ 1 đến 5.");

            var spExists = await _context.SanPhams
                .AnyAsync(x => x.Id == dto.SanPhamId && !x.XoaMem);
            if (!spExists)
                return BadRequest("Sản phẩm không tồn tại.");

            var entity = new DanhGiaSanPham
            {
                Id = Guid.NewGuid(),
                SanPhamId = dto.SanPhamId,
                NguoiDungId = dto.NguoiDungId,
                SoSao = dto.SoSao,
                NoiDung = dto.NoiDung,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            await _repo.CreateAsync(entity);
            return Ok("Đánh giá đã gửi, chờ duyệt.");
        }

        // GET: api/DanhGiaSanPham/san-pham/{sanPhamId}
        [HttpGet("list Danh gia/{sanPhamId}")]
        public async Task<ActionResult<object>> GetBySanPham(Guid sanPhamId)
        {
            var list = await _repo.GetBySanPhamIdAsync(sanPhamId);
            var avg = await _repo.TinhDiemTrungBinhAsync(sanPhamId);

            return Ok(new
            {
                DiemTrungBinh = Math.Round(avg, 1),
                TongDanhGia = list.Count,
                DanhGias = list.Select(x => new DanhGiaResponseDto
                {
                    Id = x.Id,
                    SoSao = x.SoSao,
                    NoiDung = x.NoiDung,
                    Likes = x.Likes,
                    CreatedAt = x.CreatedAt,
                    TenNguoiDung = x.NguoiDung?.HoTen
                })
            });
        }

        // POST: api/DanhGiaSanPham/{id}/like
        [HttpPost("{id}/like")]
        public async Task<IActionResult> Like(Guid id)
        {
            var ok = await _repo.LikeAsync(id);

            if (!ok)
                return NotFound("Không tìm thấy đánh giá.");

            return Ok(new
            {
                Message = "+1 Like"
            });
        }


    }

}
