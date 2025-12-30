using DATN.Model;

namespace DATN.Repository
{
    public interface IMaQrLoHangRepository
    {
        Task<MaQrLoHang> CreateForLoHangAsync(Guid loHangId, string? ghiChu = null);
        Task<MaQrLoHang?> GetByIdAsync(Guid id);
        Task<MaQrLoHang?> GetByLoHangIdAsync(Guid loHangId);
    }
}
