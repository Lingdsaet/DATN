using DATN.Model;
using DATN.Model.DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class DanhGiaSanPhamRepository : IDanhGiaSanPhamRepository
    {
        private readonly QR_DATNContext _context;

        public DanhGiaSanPhamRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<DanhGiaSanPham> CreateAsync(DanhGiaSanPham entity)
        {
            _context.DanhGiaSanPhams.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<DanhGiaSanPham>> GetBySanPhamIdAsync(Guid sanPhamId)
        {
            return await _context.DanhGiaSanPhams
                .Include(x => x.NguoiDung)
                .Where(x => x.SanPhamId == sanPhamId &&
                            !x.XoaMem)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> DuyetAsync(Guid id, string trangThai)
        {
            var entity = await _context.DanhGiaSanPhams.FindAsync(id);
            if (entity == null) return false;

            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<double> TinhDiemTrungBinhAsync(Guid sanPhamId)
        {
            return await _context.DanhGiaSanPhams
                .Where(x => x.SanPhamId == sanPhamId &&
                            !x.XoaMem)
                .AverageAsync(x => (double?)x.SoSao) ?? 0;
        }
        public async Task<bool> LikeAsync(Guid danhGiaId)
        {
            var entity = await _context.DanhGiaSanPhams
                .FirstOrDefaultAsync(x => x.Id == danhGiaId && !x.XoaMem);

            if (entity == null)
                return false;

            entity.Likes += 1;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

    }

}
