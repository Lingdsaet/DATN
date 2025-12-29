using DATN.Model;

namespace DATN.Repository
{
    public interface IMaQrSanPhamRepository
    {
        Task<MaQrSanPham> CreateForSanPhamAsync(Guid sanPhamId, string? ghiChu = null);
        Task<MaQrSanPham?> GetByIdAsync(Guid id);
        Task<MaQrSanPham?> GetByLoHangIdAsync(Guid sanPhamId);
    }
}
