using DATN.Model;

namespace DATN.Repository
{
    public interface INguoiTieuDungRepository
    {
        Task<NguoiTieuDung?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<NguoiTieuDung> CreateAsync(NguoiTieuDung ntd);
    }
}
