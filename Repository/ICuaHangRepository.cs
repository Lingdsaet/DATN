using DATN.Model;

namespace DATN.Repository
{
    public interface ICuaHangRepository
    {
        Task<CuaHang?> GetByIdAsync(Guid id);
        Task<List<CuaHang>> GetByDoanhNghiepIdAsync(Guid doanhNghiepId);
        Task<CuaHang> CreateAsync(CuaHang cuaHang);
        Task<CuaHang?> UpdateAsync(CuaHang cuaHang);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
