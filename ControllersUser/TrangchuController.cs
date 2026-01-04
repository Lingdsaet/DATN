using DATN.Model;
using DATN.ReponseDto;
using DATN.Repository;
using DATN.RequestDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DATN.CotrollerBusiness;

namespace DATN.ControllersUser
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrangchuController : ControllerBase
    {
        private readonly ISanPhamRepository _sanPhamRepository;

        public TrangchuController(ISanPhamRepository sanPhamRepository)
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

        // GET: api/SanPhams/tim-kiem
        [HttpGet("tim-kiem")]
        public async Task<ActionResult<IEnumerable<SanPhamSearchResultDto>>> SearchQuick([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword không được để trống.");

            var result = await _sanPhamRepository.SearchAsync(keyword);
            return Ok(result);
        }
    }
}