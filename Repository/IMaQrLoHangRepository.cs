using DATN.Model;

namespace DATN.Repository
{
    public interface IMaQrLoHangRepository
    {
        // Tạo QR tự động cho 1 lô hàng
        Task<MaQrLoHang> CreateForLoHangAsync(Guid loHangId, string? ghiChu = null);

        // Nếu muốn dùng thêm:
        Task<MaQrLoHang?> GetByIdAsync(Guid id);
        Task<MaQrLoHang?> GetByLoHangIdAsync(Guid loHangId);
    }
}
