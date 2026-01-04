using DATN.CotrollerBusiness;
using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;

namespace DATN.ControllersAdmin
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrangchuController : ControllerBase
    {
        private readonly ITinTucRepository _tinTucRepository;

        public TrangchuController(ISanPhamRepository sanPhamRepository, ITinTucRepository tinTucRepository)
        {
            _tinTucRepository = tinTucRepository;
        }

        //Admin - danh sách
        [HttpGet("list")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tinTucRepository.GetAllAsync());
        }

        // Admin - tạo
        [HttpPost]
        public async Task<IActionResult> Create(TinTucCreateUpdateDto dto)
        {
            var entity = new TinTuc
            {
                Id = Guid.NewGuid(),
                TieuDe = dto.TieuDe,
                TomTat = dto.TomTat,
                NoiDung = dto.NoiDung,
                HinhAnhUrl = dto.HinhAnhUrl,
                NoiBat = dto.NoiBat,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            await _tinTucRepository.CreateAsync(entity);
            return Ok(entity.Id);
        }

        // Admin - sửa
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TinTucCreateUpdateDto dto)
        {
            var entity = await _tinTucRepository.UpdateAsync(id, dto);
            if (entity == null) return NotFound();
            return Ok();
        }

        // Admin - xoá mềm
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _tinTucRepository.SoftDeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    

    }
}