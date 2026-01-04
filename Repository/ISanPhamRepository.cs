using DATN.Model;
using DATN.ReponseDto;
using DATN.RequestDto;

namespace DATN.Repository
{
    public interface ISanPhamRepository
    {
        Task<List<SanPhamSearchResultDto>> SearchAsync(string keyword);
        Task<SanPham?> GetByIdAsync(Guid id);
        Task<List<SanPham>> GetByDoanhNghiepIdAsync(Guid doanhNghiepId);
        Task<SanPham> AddAsync(SanPham sanPham);
        Task<SanPham?> UpdateAsync(SanPham sanPham);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<List<SanPham>> GetAllAsync();
    }
}
