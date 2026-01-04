using DATN.Model;
using DATN.RequestDto;

namespace DATN.Repository
{
    public interface ITinTucRepository
    {
        Task<List<TinTuc>> GetTrangChuAsync(int take);
        Task<TinTuc?> GetByIdAsync(Guid id);
        Task<List<TinTuc>> GetAllAsync();
        Task<TinTuc> CreateAsync(TinTuc entity);
        Task<TinTuc?> UpdateAsync(Guid id, TinTucCreateUpdateDto dto);
        Task<bool> SoftDeleteAsync(Guid id);
    }

}
