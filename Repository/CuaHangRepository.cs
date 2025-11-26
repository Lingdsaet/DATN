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

        public async Task<CuaHang?> GetByIdAsync(Guid id)
        {
            return await _context.CuaHangs
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<List<CuaHang>> GetByDoanhNghiepIdAsync(Guid doanhNghiepId)
        {
            return await _context.CuaHangs
                .Where(x => x.DoanhNghiepId == doanhNghiepId && !x.XoaMem)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<CuaHang> CreateAsync(CuaHang cuaHang)
        {
            _context.CuaHangs.Add(cuaHang);
            await _context.SaveChangesAsync();
            return cuaHang;
        }

        public async Task<CuaHang?> UpdateAsync(CuaHang cuaHang)
        {
            _context.CuaHangs.Update(cuaHang);
            await _context.SaveChangesAsync();
            return cuaHang;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.CuaHangs
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);

            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
