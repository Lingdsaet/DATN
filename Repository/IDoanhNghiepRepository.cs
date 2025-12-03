using DATN.Model;

namespace DATN.Repository
{
    public interface IDoanhNghiepRepository
    {
        Task<DoanhNghiep?> GetByIdAsync(Guid id);
        Task<List<DoanhNghiep>> GetAllAsync(string? trangThai);
        Task<DoanhNghiep> CreateAsync(DoanhNghiep dn);
        Task<DoanhNghiep?> UpdateAsync(DoanhNghiep dn);
        Task<bool> SoftDeleteAsync(Guid id);
        Task AddAsync(DoanhNghiep dn);
    }
}
