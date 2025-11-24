using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class NguoiTieuDungRepository : INguoiTieuDungRepository
    {
        private readonly QR_DATNContext _context;

        public NguoiTieuDungRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<NguoiTieuDung?> GetByEmailAsync(string email)
        {
            return await _context.NguoiTieuDungs
                .FirstOrDefaultAsync(x => x.Email == email && !x.XoaMem == false);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.NguoiTieuDungs
                .AnyAsync(x => x.Email == email && !x.XoaMem);
        }

        public async Task<NguoiTieuDung> CreateAsync(NguoiTieuDung ntd)
        {
            _context.NguoiTieuDungs.Add(ntd);
            await _context.SaveChangesAsync();
            return ntd;
        }
    }
}
