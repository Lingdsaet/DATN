using DATN.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace DATN.Repository
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly QR_DATNContext _context;

        public NguoiDungRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _context.NguoiDungs.AnyAsync(x => x.Email == email);
        }

        public Task<bool> PhoneExistsAsync(string phone)
        {
            return _context.NguoiDungs.AnyAsync(x => x.DienThoai == phone);
        }

        public Task AddAsync(NguoiDung entity)
        {
            return _context.NguoiDungs.AddAsync(entity).AsTask();
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public Task<NguoiDung?> GetByEmailOrPhoneAsync(string key)
        {
            return _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Email == key || x.DienThoai == key);
        }

        // 👉 lấy danh sách mã vai trò của user
        public Task<List<string>> GetRoleCodesAsync(Guid nguoiDungId)
        {
            return _context.NguoiDungVaiTro
                .Where(ndvt => ndvt.NguoiDungId == nguoiDungId)
                .Join(
                    _context.VaiTros,
                    ndvt => ndvt.VaiTroId,
                    vt => vt.Id,
                    (ndvt, vt) => vt.Ma
                )
                .ToListAsync();
        }
        public Task<NguoiDung?> GetByIdAsync(Guid id)
        {
            return _context.NguoiDungs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateAsync(NguoiDung user)
        {
            _context.NguoiDungs.Update(user);
            return Task.CompletedTask;
        }
    }

    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly QR_DATNContext _context;

        public VaiTroRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public Task<VaiTro?> GetByMaAsync(string ma)
        {
            return _context.VaiTros.FirstOrDefaultAsync(x => x.Ma == ma);
        }
    }

    public class NguoiDungVaiTroRepository : INguoiDungVaiTroRepository
    {
        private readonly QR_DATNContext _context;

        public NguoiDungVaiTroRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public Task AddAsync(NguoiDung_VaiTro mapping)
        {
            return _context.NguoiDungVaiTro.AddAsync(mapping).AsTask();
        }
    
    public Task<NguoiDung?> GetByIdAsync(Guid id)
        {
            return _context.NguoiDungs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateAsync(NguoiDung user)
        {
            _context.NguoiDungs.Update(user);
            return Task.CompletedTask;
        }

        public Task RemoveAllRolesOfUserAsync(Guid nguoiDungId)
        {
            var list = _context.NguoiDungVaiTro
                .Where(x => x.NguoiDungId == nguoiDungId);

            _context.NguoiDungVaiTro.RemoveRange(list);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
