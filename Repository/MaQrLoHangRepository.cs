using DATN.Model;
using DATN.Utils;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class MaQrLoHangRepository : IMaQrLoHangRepository
    {
        private readonly QR_DATNContext _context;
        private readonly IQrCodeService _qrCodeService;
        private readonly IFirebaseService _firebaseService;
        


        public MaQrLoHangRepository(QR_DATNContext context, IFirebaseService firebaseService, IQrCodeService qrCodeService)
        {
            _context = context;
            _firebaseService = firebaseService;
            _qrCodeService = qrCodeService;
        }

        public async Task<MaQrLoHang> CreateForLoHangAsync(Guid loHangId, string? ghiChu = null)
        {
            var now = DateTime.UtcNow;

            // Id QR dùng luôn làm token mã
            var qrId = Guid.NewGuid();
            var qrContent = $"https://yourdomain.com/truy-xuat/lo-hang/{loHangId}"; // link trang truy xuát

            //  Tạo ảnh QR
            var qrBytes = _qrCodeService.GenerateQrPng(qrContent);

            //  UPLOAD FIREBASE 
            var qrImageUrl = await _firebaseService
                .UploadQrImageAsync(qrBytes, loHangId.ToString());

            //  Tạo entity QR
            var entity = new MaQrLoHang
            {
                Id = Guid.NewGuid(),
                LoHangId = loHangId,
                MaQr = qrContent,
                QrImageUrl = qrImageUrl,  
                TrangThai = "ACTIVE",
                GhiChu = ghiChu,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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
