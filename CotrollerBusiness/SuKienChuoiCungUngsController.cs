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

namespace DATN.CotrollerBusiness
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuKienChuoiCungUngsController : ControllerBase
    {
        private readonly ISuKienChuoiCungUngRepository _repo;

        public SuKienChuoiCungUngsController(ISuKienChuoiCungUngRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("lo-hang/{loHangId}")]
        public async Task<ActionResult<IEnumerable<SuKienChuoiCungUngResponseDto>>> GetByLoHang(Guid loHangId)
        {
            var list = await _repo.GetByLoHangAsync(loHangId);

            var result = list.Select(x => new SuKienChuoiCungUngResponseDto
            {
                Id = x.Id,
                LoHangId = x.LoHangId,
                LoaiSuKien = x.LoaiSuKien,
                TenLoaiSuKien = x.LoaiSuKienNavigation?.MoTa,

                ThoiGian = x.ThoiGian,
                DonViThucHien = x.DonViThucHien,

                DiaDiemId = x.DiaDiemId,
                TenDiaDiem = x.DiaDiem?.Ten,

                TaiLieuDinhKemUrl = x.TaiLieuDinhKemUrl,
                MoTa = x.MoTa
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuKienChuoiCungUngResponseDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new SuKienChuoiCungUngResponseDto
            {
                Id = x.Id,
                LoHangId = x.LoHangId,
                LoaiSuKien = x.LoaiSuKien,
                TenLoaiSuKien = x.LoaiSuKienNavigation?.MoTa,
                ThoiGian = x.ThoiGian,
                DonViThucHien = x.DonViThucHien,
                DiaDiemId = x.DiaDiemId,
                TenDiaDiem = x.DiaDiem?.Ten,
                TaiLieuDinhKemUrl = x.TaiLieuDinhKemUrl,
                MoTa = x.MoTa
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create(SuKienChuoiCungUngRequestDto dto)
        {
            var entity = new SuKienChuoiCungUng
            {
                Id = Guid.NewGuid(),
                LoHangId = dto.LoHangId,
                LoaiSuKien = dto.LoaiSuKien,
                ThoiGian = dto.ThoiGian,
                DonViThucHien = dto.DonViThucHien,
                DiaDiemId = dto.DiaDiemId,
                TaiLieuDinhKemUrl = dto.TaiLieuDinhKemUrl,
                MoTa = dto.MoTa,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            await _repo.AddAsync(entity);
            return Ok(new { Success = true });
        }

    }
}