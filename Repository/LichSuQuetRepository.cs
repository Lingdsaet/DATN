using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class LichSuQuetRepository : ILichSuQuetRepository
    {
        private readonly QR_DATNContext _context;

        public LichSuQuetRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<List<LichSuQuet>> GetByNguoiTieuDungIdAsync(Guid nguoiTieuDungId)
        {
            return await _context.LichSuQuets
                .Where(x => x.NguoiTieuDungId == nguoiTieuDungId)
                .Include(x => x.MaQrLoHang)               // tên navigation có thể là MaQrLoHang hoặc MaQR_LoHang
                    .ThenInclude(q => q.LoHang)
                        .ThenInclude(l => l.SanPham)
                            .ThenInclude(sp => sp.DoanhNghiep)
                .OrderByDescending(x => x.ThoiGian)
                .ToListAsync();
        }

        public async Task<LichSuQuet?> GetByIdAsync(Guid id)
        {
            return await _context.LichSuQuets
                .Include(x => x.MaQrLoHang)
                    .ThenInclude(q => q.LoHang)
                        .ThenInclude(l => l.SanPham)
                            .ThenInclude(sp => sp.DoanhNghiep)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
