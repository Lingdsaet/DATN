using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class LoHangRepository : ILoHangRepository
    {
        private readonly QR_DATNContext _context;

        public LoHangRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<LoHang?> GetByIdAsync(Guid id)
        {
            return await _context.LoHangs
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<LoHang> CreateAsync(LoHang loHang)
        {
            _context.LoHangs.Add(loHang);
            await _context.SaveChangesAsync();
            return loHang;
        }

        public async Task<LoHang?> UpdateAsync(LoHang loHang)
        {
            _context.LoHangs.Update(loHang);
            await _context.SaveChangesAsync();
            return loHang;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.LoHangs.FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
