using DATN.Model;
using Microsoft.EntityFrameworkCore;


namespace DATN.Repository
{
    public class QrScanRepository : IQrScanRepository
    {
        private readonly QR_DATNContext _context;

        public QrScanRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<MaQrLoHang?> GetMaQrWithRelationsAsync(string maQr)
        {
            return await _context.MaQrLoHangs
                .Include(q => q.LoHang)
                    .ThenInclude(l => l.SanPham)
                        .ThenInclude(sp => sp.DoanhNghiep)
                .FirstOrDefaultAsync(q => q.MaQr == maQr && !q.XoaMem);
        }
        public async Task<MaQrSanPham?> GetMaQrSanPhamAsync(string maQr)
        {
            return await _context.MaQrSanPhams
                .Include(q => q.SanPham)
                    .ThenInclude(sp => sp.DoanhNghiep)
                .FirstOrDefaultAsync(q => q.MaQr == maQr && !q.XoaMem);
        }

        public async Task<List<SuKienChuoiCungUng>> GetSuKienByLoHangIdAsync(Guid loHangId)
        {
            return await _context.SuKienChuoiCungUngs
                .Where(x => x.LoHangId == loHangId && !x.XoaMem)
                .OrderBy(x => x.ThoiGian)
                .ToListAsync();
        }

        public async Task<LichSuQuet> CreateLanQuetAsync(LichSuQuet lanQuet)
        {
            _context.LichSuQuets.Add(lanQuet);
            await _context.SaveChangesAsync();
            return lanQuet;
        }
    }
}
