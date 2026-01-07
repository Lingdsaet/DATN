using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using DATN.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.CotrollerBusiness
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class CuaHangsController : ControllerBase
    {
        private readonly ICuaHangRepository _repo;
        private readonly QR_DATNContext _context;

        public CuaHangsController(ICuaHangRepository repo, QR_DATNContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("doanh-nghiep/{doanhNghiepId}")]
        public async Task<ActionResult<IEnumerable<CuaHangResponseDto>>> GetByDoanhNghiep(Guid doanhNghiepId)
        {
            var list = await _repo.GetByDoanhNghiepAsync(doanhNghiepId);

            var result = list.Select(x => new CuaHangResponseDto
            {
                Id = x.Id,
                Ten = x.Ten,
                LienHe = x.LienHe,
                DiaDiem = DiaDiemMapper.ToDto(x.DiaDiem)
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuaHangResponseDto>> GetById(Guid id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new CuaHangResponseDto
            {
                Id = x.Id,
                Ten = x.Ten,
                TenDoanhNghiep = x.DoanhNghiep?.Ten,
                LienHe = x.LienHe,
                DiaDiem = DiaDiemMapper.ToDto(x.DiaDiem)
            });
        }

        [HttpPost("themcuahang")]
        public async Task<IActionResult> Create([FromBody] CuaHangCreateRequestDto dto)
        {
            //  Check doanh nghiệp
            var dnExists = await _context.DoanhNghieps
                .AnyAsync(x => x.Id == dto.DoanhNghiepId && !x.XoaMem);

            if (!dnExists)
                return BadRequest("Doanh nghiệp không tồn tại.");

            //  Validate địa điểm
            if (dto.DiaDiem == null)
                return BadRequest("Phải nhập thông tin địa điểm.");

            //  Tạo địa điểm mới
            var diaDiem = new DiaDiem
            {
                Id = Guid.NewGuid(),
                Ten = dto.DiaDiem.Ten,
                Tinh = dto.DiaDiem.Tinh,
                Huyen = dto.DiaDiem.Huyen,
                Xa = dto.DiaDiem.Xa,
                DiaChi = dto.DiaDiem.DiaChi,
                ViDo = dto.DiaDiem.ViDo,
                KinhDo = dto.DiaDiem.KinhDo,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            _context.DiaDiems.Add(diaDiem);
            await _context.SaveChangesAsync();

            // Tạo cửa hàng
            var cuaHang = new CuaHang
            {
                Id = Guid.NewGuid(),
                DoanhNghiepId = dto.DoanhNghiepId,
                Ten = dto.Ten,
                LienHe = dto.LienHe,
                DiaDiemId = diaDiem.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            _context.CuaHangs.Add(cuaHang);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                CuaHangId = cuaHang.Id,
                DiaDiemId = diaDiem.Id
            });
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, CuaHangRequestDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();

            entity.Ten = dto.Ten;
            entity.LienHe = dto.LienHe;
            entity.DiaDiemId = dto.DiaDiemId;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity);
            return Ok(new { Success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _repo.SoftDeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    
        public static class DiaDiemMapper
        {
            public static DiaDiemDto? ToDto(DiaDiem? entity)
            {
                if (entity == null) return null;

                return new DiaDiemDto
                {
                    Id = entity.Id,
                    Ten = entity.Ten,
                    DiaChi = $"{entity.DiaChi}, {entity.Xa}, {entity.Huyen}, {entity.Tinh}",
                    ViDo = entity.ViDo,
                    KinhDo = entity.KinhDo
                };
            }
        }

    }
}
