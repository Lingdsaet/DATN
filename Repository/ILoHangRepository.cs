using DATN.Model;

namespace DATN.Repository
{
    public interface ILoHangRepository
    {
        Task<LoHang?> GetByIdAsync(Guid id);
        Task<LoHang> CreateAsync(LoHang loHang);
        Task<LoHang?> UpdateAsync(LoHang loHang);
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
