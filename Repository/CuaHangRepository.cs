using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class CuaHangRepository : ICuaHangRepository
    {
        private readonly QR_DATNContext _context;

        public CuaHangRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<List<CuaHang>> GetByDoanhNghiepAsync(Guid doanhNghiepId)
        {
            return await _context.CuaHangs
                .Include(x => x.DiaDiem)
                .Where(x => x.DoanhNghiepId == doanhNghiepId && !x.XoaMem)
                .OrderBy(x => x.Ten)
                .ToListAsync();
        }

        public async Task<CuaHang?> GetByIdAsync(Guid id)
        {
            return await _context.CuaHangs
                .Include(x => x.DoanhNghiep)
                .Include(x => x.DiaDiem)
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<CuaHang> AddAsync(CuaHang entity)
        {
            _context.CuaHangs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<CuaHang> UpdateAsync(CuaHang entity)
        {
            _context.CuaHangs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.CuaHangs.FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
