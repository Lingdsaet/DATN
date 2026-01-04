using DATN.Model;
using DATN.RequestDto;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class TinTucRepository : ITinTucRepository
    {
        private readonly QR_DATNContext _context;

        public TinTucRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<List<TinTuc>> GetTrangChuAsync(int take)
        {
            return await _context.TinTucs
                .Where(x => !x.XoaMem)
                .OrderByDescending(x => x.NoiBat)
                .ThenByDescending(x => x.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        public async Task<TinTuc?> GetByIdAsync(Guid id)
        {
            return await _context.TinTucs
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<List<TinTuc>> GetAllAsync()
        {
            return await _context.TinTucs
                .Where(x => !x.XoaMem)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<TinTuc> CreateAsync(TinTuc entity)
        {
            _context.TinTucs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TinTuc?> UpdateAsync(Guid id, TinTucCreateUpdateDto dto)
        {
            var entity = await _context.TinTucs.FindAsync(id);
            if (entity == null || entity.XoaMem) return null;

            entity.TieuDe = dto.TieuDe;
            entity.TomTat = dto.TomTat;
            entity.NoiDung = dto.NoiDung;
            entity.HinhAnhUrl = dto.HinhAnhUrl;
            entity.NoiBat = dto.NoiBat;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.TinTucs.FindAsync(id);
            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
