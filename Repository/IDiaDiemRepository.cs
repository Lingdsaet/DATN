using DATN.Model;

namespace DATN.Repository
{
    public interface IDiaDiemRepository
    {
        Task<List<DiaDiem>> GetAllAsync();
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
