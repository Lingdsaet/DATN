namespace DATN.Repository
{
    using DATN.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INguoiDungRepository
    {
        Task<bool> EmailExistsAsync(string email);
        Task<bool> PhoneExistsAsync(string phone);

        Task AddAsync(NguoiDung entity);
        Task SaveChangesAsync();
        Task<NguoiDung?> GetByEmailOrPhoneAsync(string key);
        Task<List<string>> GetRoleCodesAsync(Guid nguoiDungId);
        Task<NguoiDung?> GetByIdAsync(Guid id);
        Task UpdateAsync(NguoiDung user);

    }

    public interface IVaiTroRepository
    {
        Task<VaiTro?> GetByMaAsync(string ma);
    }

    public interface INguoiDungVaiTroRepository
    {
        Task AddAsync(NguoiDungVaiTro mapping);
        Task RemoveAllRolesOfUserAsync(Guid nguoiDungId);
        Task SaveChangesAsync();
    }

}
