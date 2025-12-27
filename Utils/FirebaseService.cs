using Firebase.Storage;
namespace DATN.Utils
{
    public class FirebaseService : IFirebaseService
    {
        private readonly string _bucket = "fir-9a230.appspot.com";

        public async Task<string> UploadImageToFirebase(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            using (var stream = file.OpenReadStream())
            {
                var task = new FirebaseStorage(_bucket)
                    .Child("SanPhamImages")
                    .Child(fileName)
                    .PutAsync(stream);

                return await task;
            }
        }
        public async Task<string> UploadQrImageAsync(
        byte[] imageBytes,
        string fileName)
        {
            using var stream = new MemoryStream(imageBytes);

            var task = new FirebaseStorage(_bucket)
                .Child("qr")
                .Child("lo-hang")
                .Child($"{fileName}.png")
                .PutAsync(stream);

            return await task; // 🔥 URL ảnh QR
        }

    }
}
