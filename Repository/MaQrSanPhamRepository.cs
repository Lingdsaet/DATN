using DATN.Model;
using DATN.Utils;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class MaQrSanPhamRepository : IMaQrSanPhamRepository
    {
        private readonly QR_DATNContext _context;
        private readonly IQrCodeService _qrCodeService;
        private readonly IFirebaseService _firebaseService;



        public MaQrSanPhamRepository(QR_DATNContext context, IFirebaseService firebaseService, IQrCodeService qrCodeService)
        {
            _context = context;
            _firebaseService = firebaseService;
            _qrCodeService = qrCodeService;
        }

        public async Task<MaQrSanPham> CreateForSanPhamAsync(Guid sanPhamId, string? ghiChu = null)
        {
            var now = DateTime.UtcNow;

            // Id QR dùng luôn làm token mã
            var qrId = Guid.NewGuid();
            var qrContent = $"{sanPhamId}";

            //  Tạo ảnh QR
            var qrBytes = _qrCodeService.GenerateQrPng(qrContent);

            //  UPLOAD FIREBASE 
            var qrImageUrl = await _firebaseService
                .UploadQrImageAsync(qrBytes, sanPhamId.ToString());

            //  Tạo entity QR
            var entity = new MaQrSanPham
            {
                Id = Guid.NewGuid(),
                SanPhamId = sanPhamId,
                MaQr = qrContent,
                QrImageUrl = qrImageUrl,
                TrangThai = "ACTIVE",
                GhiChu = ghiChu,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                XoaMem = false
            };

            _context.MaQrSanPhams.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<MaQrSanPham?> GetByIdAsync(Guid id)
        {
            return await _context.MaQrSanPhams
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<MaQrSanPham?> GetByLoHangIdAsync(Guid loHangId)
        {
            return await _context.MaQrSanPhams
                .FirstOrDefaultAsync(x => x.SanPhamId == loHangId && !x.XoaMem);
        }
    }
}
