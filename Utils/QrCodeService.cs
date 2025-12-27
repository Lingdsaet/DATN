using QRCoder;

namespace DATN.Utils
{
    public interface IQrCodeService
    {
        byte[] GenerateQrPng(string content);
    }
    public class QrCodeService : IQrCodeService
    {
        public byte[] GenerateQrPng(string content)
        {
            using var generator = new QRCodeGenerator();
            using var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(data);
            return qrCode.GetGraphic(20); // 20 = nét đậm → in rõ
        }
    }
}
