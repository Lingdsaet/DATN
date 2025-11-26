using DATN.Model;

namespace DATN.Repository
{
    public interface IBaoCaoNguoiDungRepository
    {
        Task<BaoCaoNguoiDung> CreateAsync(BaoCaoNguoiDung bc);
        Task<BaoCaoNguoiDung?> GetByIdAsync(Guid id);
        Task<List<BaoCaoNguoiDung>> GetListAsync(Guid? nguoiTieuDungId, string? trangThaiXuLy);
        Task<BaoCaoNguoiDung?> UpdateAsync(BaoCaoNguoiDung bc);
    }
}
