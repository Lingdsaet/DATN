using DATN.Model;

namespace DATN.Repository
{
    public interface IYeuCauDangKyDnRepository
    {
        Task<YeuCauDangKyDn> CreateAsync(YeuCauDangKyDn yc);
        Task<YeuCauDangKyDn?> GetByIdAsync(Guid id);
        Task<List<YeuCauDangKyDn>> GetAllAsync(string? trangThai);
        Task<YeuCauDangKyDn?> UpdateAsync(YeuCauDangKyDn yc);
    }
}
