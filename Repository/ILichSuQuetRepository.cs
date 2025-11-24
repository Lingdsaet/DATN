using DATN.Model;

namespace DATN.Repository
{
    public interface ILichSuQuetRepository
    {
        Task<List<LichSuQuet>> GetByNguoiTieuDungIdAsync(Guid nguoiTieuDungId);
        Task<LichSuQuet?> GetByIdAsync(Guid id);
    }
}
