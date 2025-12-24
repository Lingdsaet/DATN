using DATN.Model;

namespace DATN.Repository
{
    public interface ISanPhamRepository
    {
        Task<SanPham?> GetByIdAsync(Guid id);
        Task<List<SanPham>> GetByDoanhNghiepIdAsync(Guid doanhNghiepId);
        Task<SanPham> AddAsync(SanPham sanPham);
        Task<SanPham?> UpdateAsync(SanPham sanPham);
        Task<bool> SoftDeleteAsync(Guid id);
        Task<List<SanPham>> GetAllAsync();
    }
}
