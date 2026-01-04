using DATN.Model;
using DATN.RequestDto;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System;
using DATN.Utils;


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

        // lấy danh sách mã vai trò của user
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
        //public Task<NguoiDung?> GetByIdAsync(Guid id)
        //{
        //    return _context.NguoiDungs.FirstOrDefaultAsync(x => x.Id == id);
        //}

        public Task UpdateAsync(NguoiDung user)
        {
            _context.NguoiDungs.Update(user);
            return Task.CompletedTask;
        }
        public async Task<List<NguoiDung>> GetAllAsync()
        {
            return await _context.NguoiDungs
                .Where(x => !x.XoaMem)
                .Include(x => x.NguoiDungVaiTros)
                    .ThenInclude(x => x.VaiTro)
                .ToListAsync();
        }

        public async Task<NguoiDung?> GetByIdAsync(Guid id)
        {
            return await _context.NguoiDungs
                .Include(x => x.NguoiDungVaiTros)
                    .ThenInclude(x => x.VaiTro)
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task UpdateAdminAsync(NguoiDung entity)
        {
            _context.NguoiDungs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var user = await _context.NguoiDungs.FindAsync(id);
            if (user == null) return;

            user.XoaMem = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

        public async Task GanVaiTroAsync(Guid nguoiDungId, List<byte> vaiTroIds)
        {
            var oldRoles = _context.NguoiDungVaiTro
                .Where(x => x.NguoiDungId == nguoiDungId);

            _context.NguoiDungVaiTro.RemoveRange(oldRoles);

            foreach (var roleId in vaiTroIds)
            {
                _context.NguoiDungVaiTro.Add(new NguoiDungVaiTro
                {
                    NguoiDungId = nguoiDungId,
                    VaiTroId = roleId
                });
            }

            await _context.SaveChangesAsync();
        }

        // đổi mật khẩu
        public async Task<bool> DoiMatKhauAsync(Guid nguoiDungId, DoiMatKhauDto dto)
        {
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Id == nguoiDungId && !x.XoaMem);

            if (user == null)
                return false;

            // Kiểm tra mật khẩu cũ
            var isOldPasswordCorrect =
                PasswordHasher.VerifyPassword(dto.MatKhauCu, user.MatKhauHash);

            if (!isOldPasswordCorrect)
                throw new Exception("Mật khẩu cũ không đúng.");

            // Validate mật khẩu mới
            if (dto.MatKhauMoi.Length < 6)
                throw new Exception("Mật khẩu mới phải có ít nhất 6 ký tự.");

            // 3Hash & lưu mật khẩu mới
            user.MatKhauHash = PasswordHasher.HashPassword(dto.MatKhauMoi);
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        // đổi thông tin (không đổi mật khẩu)
        public async Task<bool> UpdateThongTinAsync(Guid nguoiDungId, NguoiDungUpdateDto dto)
        {
            var user = await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.Id == nguoiDungId && !x.XoaMem);

            if (user == null)
                return false;

            //CẬP NHẬT THÔNG TIN 
            if (!string.IsNullOrWhiteSpace(dto.HoTen))
                user.HoTen = dto.HoTen;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                user.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.DienThoai))
                user.DienThoai = dto.DienThoai;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
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

        public async Task<List<VaiTro>> GetAllAsync()
        {
            return await _context.VaiTros
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<VaiTro?> GetByIdAsync(byte id)
        {
            return await _context.VaiTros.FindAsync(id);
        }

        public async Task<bool> ExistsByMaAsync(string ma)
        {
            return await _context.VaiTros.AnyAsync(x => x.Ma == ma);
        }

        public async Task AddAsync(VaiTro entity)
        {
            _context.VaiTros.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(byte id)
        {
            var role = await _context.VaiTros
                .Include(x => x.NguoiDungVaiTros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
                return;

          
            _context.NguoiDungVaiTro.RemoveRange(role.NguoiDungVaiTros);
            _context.VaiTros.Remove(role);

            await _context.SaveChangesAsync();
        }
    }

    public class NguoiDungVaiTroRepository : INguoiDungVaiTroRepository
    {
        private readonly QR_DATNContext _context;

        public NguoiDungVaiTroRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public Task AddAsync(NguoiDungVaiTro mapping)
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
