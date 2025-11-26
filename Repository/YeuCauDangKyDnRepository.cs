using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class YeuCauDangKyDnRepository : IYeuCauDangKyDnRepository
    {
        private readonly QR_DATNContext _context;

        public YeuCauDangKyDnRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<YeuCauDangKyDn> CreateAsync(YeuCauDangKyDn yc)
        {
            _context.YeuCauDangKyDns.Add(yc);
            await _context.SaveChangesAsync();
            return yc;
        }

        public async Task<YeuCauDangKyDn?> GetByIdAsync(Guid id)
        {
            return await _context.YeuCauDangKyDns
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<YeuCauDangKyDn>> GetAllAsync(string? trangThai)
        {
            var query = _context.YeuCauDangKyDns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(x => x.TrangThai == trangThai);
            }

            return await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<YeuCauDangKyDn?> UpdateAsync(YeuCauDangKyDn yc)
        {
            _context.YeuCauDangKyDns.Update(yc);
            await _context.SaveChangesAsync();
            return yc;
        }
    }
}
