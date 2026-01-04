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
    public class BaoCaoNguoiDungController : ControllerBase
    {
        private readonly IBaoCaoNguoiDungRepository _repo;

        public BaoCaoNguoiDungController(IBaoCaoNguoiDungRepository repo)
        {
            _repo = repo;
        }

        // Người dùng gửi báo cáo
        // POST: api/BaoCaoNguoiDung
        [HttpPost]
        public async Task<ActionResult<BaoCaoNguoiDungResponseDto>> Create(
            [FromBody] BaoCaoNguoiDungCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!request.MaQrLoHangId.HasValue && !request.LanQuetId.HasValue)
                return BadRequest("Cần có ít nhất MaQrLoHangId hoặc LanQuetId.");

            var now = DateTime.UtcNow;

            var entity = new BaoCaoNguoiDung
            {
                Id = Guid.NewGuid(),
                NguoiDungId = request.NguoiTieuDungId,
                MaQrLoHangId = request.MaQrLoHangId, // sửa cho đúng property của entity
                LanQuetId = request.LanQuetId,
                ThoiGian = now,
                MucDo = request.MucDo,
                MoTa = request.MoTa,
                HinhAnhUrl = request.HinhAnhUrl,
                TrangThaiXuLy = "OPEN",
                GhiChuXuLy = null
            };

            entity = await _repo.CreateAsync(entity);

            var dto = MapToDto(entity);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // Admin/người dùng xem danh sách
        // GET: api/BaoCaoNguoiDung?nguoiTieuDungId=...&trangThaiXuLy=OPEN
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BaoCaoNguoiDungResponseDto>>> GetList(
            [FromQuery] Guid? nguoiTieuDungId,
            [FromQuery] string? trangThaiXuLy)
        {
            var list = await _repo.GetListAsync(nguoiTieuDungId, trangThaiXuLy);
            var result = list.Select(MapToDto);
            return Ok(result);
        }

        // Xem chi tiết 1 báo cáo
        // GET: api/BaoCaoNguoiDung/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BaoCaoNguoiDungResponseDto>> GetById(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy báo cáo.");

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        // Admin cập nhật trạng thái xử lý
        // PUT: api/BaoCaoNguoiDung/{id}/xu-ly
        [HttpPut("Admin/{id}/xu-ly")]
        public async Task<ActionResult<BaoCaoNguoiDungResponseDto>> UpdateStatus(
            Guid id,
            [FromBody] BaoCaoNguoiDungUpdateStatusRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                return NotFound("Không tìm thấy báo cáo.");

            entity.TrangThaiXuLy = request.TrangThaiXuLy;
            entity.GhiChuXuLy = request.GhiChuXuLy;

            entity = await _repo.UpdateAsync(entity);

            var dto = MapToDto(entity);
            return Ok(dto);
        }

        private static BaoCaoNguoiDungResponseDto MapToDto(BaoCaoNguoiDung x)
        {
            return new BaoCaoNguoiDungResponseDto
            {
                Id = x.Id,
                NguoiTieuDungId = x.NguoiDungId,
                MaQrLoHangId = x.MaQrLoHangId, 
                LanQuetId = x.LanQuetId,
                ThoiGian = x.ThoiGian,
                MucDo = x.MucDo,
                MoTa = x.MoTa,
                HinhAnhUrl = x.HinhAnhUrl,
                TrangThaiXuLy = x.TrangThaiXuLy,
                GhiChuXuLy = x.GhiChuXuLy
            };
        }
    }
}
