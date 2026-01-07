using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class SuKienChuoiCungUngRepository : ISuKienChuoiCungUngRepository
    {
        private readonly QR_DATNContext _context;

        public SuKienChuoiCungUngRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<List<SuKienChuoiCungUng>> GetByLoHangAsync(Guid loHangId)
        {
            return await _context.SuKienChuoiCungUngs
                .Include(x => x.DiaDiem)
                .Include(x => x.LoaiSuKienNavigation)
                .Where(x => x.LoHangId == loHangId && !x.XoaMem)
                .OrderBy(x => x.ThoiGian)
                .ToListAsync();
        }

        public async Task<SuKienChuoiCungUng?> GetByIdAsync(Guid id)
        {
            return await _context.SuKienChuoiCungUngs
                .Include(x => x.DiaDiem)
                .Include(x => x.LoaiSuKienNavigation)
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<SuKienChuoiCungUng> AddAsync(SuKienChuoiCungUng entity)
        {
            _context.SuKienChuoiCungUngs.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }

}
