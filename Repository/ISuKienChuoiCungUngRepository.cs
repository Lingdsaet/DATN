using DATN.Model;

namespace DATN.Repository
{
    public interface ISuKienChuoiCungUngRepository
    {
        Task<List<SuKienChuoiCungUng>> GetByLoHangAsync(Guid loHangId);
        Task<SuKienChuoiCungUng?> GetByIdAsync(Guid id);
        Task<SuKienChuoiCungUng> AddAsync(SuKienChuoiCungUng entity);
    }

}
