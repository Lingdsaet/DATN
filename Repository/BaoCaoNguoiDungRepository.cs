using DATN.Model;
using DATN.Repository;
using Microsoft.EntityFrameworkCore;

namespace DATN1.Repository
{
    public class BaoCaoNguoiDungRepository : IBaoCaoNguoiDungRepository
    {
        private readonly QR_DATNContext _context;

        public BaoCaoNguoiDungRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<BaoCaoNguoiDung> CreateAsync(BaoCaoNguoiDung bc)
        {
            _context.BaoCaoNguoiDungs.Add(bc);
            await _context.SaveChangesAsync();
            return bc;
        }

        public async Task<BaoCaoNguoiDung?> GetByIdAsync(Guid id)
        {
            return await _context.BaoCaoNguoiDungs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<BaoCaoNguoiDung>> GetListAsync(Guid? nguoiTieuDungId, string? trangThaiXuLy)
        {
            var query = _context.BaoCaoNguoiDungs.AsQueryable();

            if (nguoiTieuDungId.HasValue)
                query = query.Where(x => x.NguoiDungId == nguoiTieuDungId.Value);

            if (!string.IsNullOrWhiteSpace(trangThaiXuLy))
                query = query.Where(x => x.TrangThaiXuLy == trangThaiXuLy);

            return await query
                .OrderByDescending(x => x.ThoiGian)
                .ToListAsync();
        }

        public async Task<BaoCaoNguoiDung?> UpdateAsync(BaoCaoNguoiDung bc)
        {
            _context.BaoCaoNguoiDungs.Update(bc);
            await _context.SaveChangesAsync();
            return bc;
        }
    }
}
