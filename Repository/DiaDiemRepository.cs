using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class DiaDiemRepository : IDiaDiemRepository
    {
        private readonly QR_DATNContext _context;

        public DiaDiemRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<List<DiaDiem>> GetAllAsync()
        {
            return await _context.DiaDiems
                .Where(x => !x.XoaMem)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
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
