using DATN.Model;
using DATN.Repository;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class DoanhNghiepRepository : IDoanhNghiepRepository
    {
        private readonly QR_DATNContext _context;

        public DoanhNghiepRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<DoanhNghiep?> GetByIdAsync(Guid id)
        {
            return await _context.DoanhNghieps
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<List<DoanhNghiep>> GetAllAsync(string? trangThai)
        {
            var query = _context.DoanhNghieps
                .Where(x => !x.XoaMem);

            if (!string.IsNullOrWhiteSpace(trangThai))
                query = query.Where(x => x.TrangThai == trangThai);

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<DoanhNghiep> CreateAsync(DoanhNghiep dn)
        {
            _context.DoanhNghieps.Add(dn);
            await _context.SaveChangesAsync();
            return dn;
        }

        public async Task<DoanhNghiep?> UpdateAsync(DoanhNghiep dn)
        {
            _context.DoanhNghieps.Update(dn);
            await _context.SaveChangesAsync();
            return dn;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.DoanhNghieps
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);

            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
