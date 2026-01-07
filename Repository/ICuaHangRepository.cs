using DATN.Model;

namespace DATN.Repository
{
    public interface ICuaHangRepository
    {
        Task<List<CuaHang>> GetByDoanhNghiepAsync(Guid doanhNghiepId);
        Task<CuaHang?> GetByIdAsync(Guid id);
        Task<CuaHang> AddAsync(CuaHang entity);
        Task<CuaHang> UpdateAsync(CuaHang entity);
        Task<bool> SoftDeleteAsync(Guid id);
    }

}
