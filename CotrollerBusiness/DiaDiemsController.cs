
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DATN.Repository;
using DATN.ReponseDto;

namespace DATN.CotrollerBusiness
{
    [ApiController]
    [Route("api/[controller]")]
public class DiaDiemsController : ControllerBase
{
    private readonly IDiaDiemRepository _repo;

    public DiaDiemsController(IDiaDiemRepository repo)
    {
        _repo = repo;
    }

        // GET: api/DiaDiems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiaDiemResponseDto>>> GetAll()
        {
            var list = await _repo.GetAllAsync();

            var result = list.Select(x => new DiaDiemResponseDto
            {
                Id = x.Id,
                Ten = x.Ten,
                DiaChiDayDu = $"{x.DiaChi}, {x.Xa}, {x.Huyen}, {x.Tinh}",
                ViDo = x.ViDo,
                KinhDo = x.KinhDo
            });

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _repo.SoftDeleteAsync(id);
            if (!ok) return NotFound();

            return NoContent();
        }
    }   
}
