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
        private readonly ITinTucRepository _tinTucRepository;

        public TrangchuController(ISanPhamRepository sanPhamRepository, ITinTucRepository tinTucRepository)
        {
            _sanPhamRepository = sanPhamRepository;
            _tinTucRepository = tinTucRepository;
        }


        [HttpGet("list_all_san_pham")]
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

        //// GET: api/SanPhams/doanh-nghiep/{enterpriseId}
        //[HttpGet("doanh-nghiep/{enterpriseId}")]
        //public async Task<ActionResult<object>> GetByEnterprise(Guid enterpriseId)
        //{
        //    var list = await _sanPhamRepository.GetAllAsync();
        //    var filtered = list.Where(x => x.DoanhNghiepId == enterpriseId).ToList();

        //    var result = filtered.Select(sp => new SanPhamResponseDto
        //    {
        //        Id = sp.Id,
        //        DoanhNghiepId = sp.DoanhNghiepId,
        //        MaSanPham = sp.MaSanPham,
        //        Ten = sp.Ten,
        //        MoTa = sp.MoTa,
        //        HinhAnhUrl = sp.HinhAnhUrl
        //    }).ToList();

        //    return Ok(result);
        //}


        // GET: api/SanPhams/tim-kiem
        [HttpGet("tim-kiem")]
        public async Task<ActionResult<IEnumerable<SanPhamSearchResultDto>>> SearchQuick([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Keyword không được để trống.");

            var result = await _sanPhamRepository.SearchAsync(keyword);
            return Ok(result);
        }
        [HttpGet("tin_tuc_trang_chu")]
        public async Task<ActionResult<IEnumerable<TinTucHomeDto>>> TrangChu()
        {
            var list = await _tinTucRepository.GetTrangChuAsync(5);
            return Ok(list.Select(x => new TinTucHomeDto
            {
                Id = x.Id,
                TieuDe = x.TieuDe,
                TomTat = x.TomTat,
                HinhAnhUrl = x.HinhAnhUrl,
                CreatedAt = x.CreatedAt
            }));
        }

        [HttpGet("chi_tiet_tin_tuc/{id}")]
        public async Task<ActionResult<TinTucDetailDto>> GetById(Guid id)
        {
            var x = await _tinTucRepository.GetByIdAsync(id);
            if (x == null) return NotFound();

            return Ok(new TinTucDetailDto
            {
                Id = x.Id,
                TieuDe = x.TieuDe,
                TomTat = x.TomTat,
                NoiDung = x.NoiDung,
                HinhAnhUrl = x.HinhAnhUrl,
                CreatedAt = x.CreatedAt
            });
        }
    }
}