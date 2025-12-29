namespace DATN.Repository
{
    using DATN.Model;

    public interface ILoaiSanPhamRepository
    {
        Task<List<LoaiSanPham>> GetAllAsync();
        Task<LoaiSanPham?> GetByIdAsync(Guid id);
        Task<bool> ExistsByMaLoaiAsync(string maLoai);
        Task AddAsync(LoaiSanPham entity);
        Task UpdateAsync(LoaiSanPham entity);
        Task<bool> SoftDeleteAsync(Guid id);
    }

}
