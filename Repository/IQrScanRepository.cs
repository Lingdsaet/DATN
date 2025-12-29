using DATN.Model;


namespace DATN.Repository
{
    public interface IQrScanRepository
    {
        Task<MaQrLoHang?> GetMaQrWithRelationsAsync(string maQr);
        Task<List<SuKienChuoiCungUng>> GetSuKienByLoHangIdAsync(Guid loHangId);
        Task<LichSuQuet> CreateLanQuetAsync(LichSuQuet lanQuet);
    }
}
