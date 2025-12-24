namespace DATN.Utils
{
    public interface IFirebaseService
    {
        Task<string> UploadImageToFirebase(IFormFile file);
    }
}
