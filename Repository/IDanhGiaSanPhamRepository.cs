using DATN.Model.DATN.Model;

namespace DATN.Repository
{
    public interface IDanhGiaSanPhamRepository
    {
        Task<DanhGiaSanPham> CreateAsync(DanhGiaSanPham entity);
        Task<List<DanhGiaSanPham>> GetBySanPhamIdAsync(Guid sanPhamId);
        Task<bool> DuyetAsync(Guid id, string trangThai);
        Task<double> TinhDiemTrungBinhAsync(Guid sanPhamId);
        Task<bool> LikeAsync(Guid danhGiaId);
    }

}
