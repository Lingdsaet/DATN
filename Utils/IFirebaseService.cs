namespace DATN.Utils
{
    public interface IFirebaseService
    {
        Task<string> UploadImageToFirebase(IFormFile file);
        Task<string> UploadQrImageAsync(byte[] imageBytes, string fileName);
    }
}
