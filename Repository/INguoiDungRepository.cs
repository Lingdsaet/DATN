namespace DATN.Repository
{
    using DATN.Model;
    using DATN.RequestDto;
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
        Task<List<NguoiDung>> GetAllAsync();
        Task UpdateAdminAsync(NguoiDung entity);
        Task SoftDeleteAsync(Guid id);
        Task<bool> UpdateThongTinAsync(Guid nguoiDungId, NguoiDungUpdateDto dto);
        Task<bool> DoiMatKhauAsync(Guid nguoiDungId, DoiMatKhauDto dto);
        Task GanVaiTroAsync(Guid nguoiDungId, List<byte> vaiTroIds);

    }

    public interface IVaiTroRepository
    {
        Task<VaiTro?> GetByMaAsync(string ma);
        Task<List<VaiTro>> GetAllAsync();
        Task<VaiTro?> GetByIdAsync(byte id);
        Task<bool> ExistsByMaAsync(string ma);
        Task AddAsync(VaiTro entity);
        Task DeleteAsync(byte id);
    }

    public interface INguoiDungVaiTroRepository
    {
        Task AddAsync(NguoiDungVaiTro mapping);
        Task RemoveAllRolesOfUserAsync(Guid nguoiDungId);
        Task SaveChangesAsync();
    }

}
