using DATN.Model;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class MaQrLoHangRepository : IMaQrLoHangRepository
    {
        private readonly QR_DATNContext _context;

        public MaQrLoHangRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<MaQrLoHang> CreateForLoHangAsync(Guid loHangId, string? ghiChu = null)
        {
            var now = DateTime.UtcNow;

            // Id QR dùng luôn làm token mã
            var qrId = Guid.NewGuid();
            var qrContent = qrId.ToString("N"); // hoặc build URL tại đây

            var entity = new MaQrLoHang
            {
                Id = qrId,
                LoHangId = loHangId,
                MaQr = qrContent,
                TrangThai = "ACTIVE",              // phải tồn tại trong DM_TrangThaiQR
                GhiChu = ghiChu,
                CreatedAt = now,
                UpdatedAt = now,
                XoaMem = false
            };

            _context.MaQrLoHangs.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<MaQrLoHang?> GetByIdAsync(Guid id)
        {
            return await _context.MaQrLoHangs
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<MaQrLoHang?> GetByLoHangIdAsync(Guid loHangId)
        {
            return await _context.MaQrLoHangs
                .FirstOrDefaultAsync(x => x.LoHangId == loHangId && !x.XoaMem);
        }
    }
}
