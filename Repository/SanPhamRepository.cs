using DATN.Model;
using DATN.ReponseDto;
using DATN.RequestDto;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly QR_DATNContext _context;

        public SanPhamRepository(QR_DATNContext context)
        {
            _context = context;
        }

        public async Task<SanPham?> GetByIdAsync(Guid id)
        {
            return await _context.SanPhams
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
        }

        public async Task<List<SanPham>> GetByDoanhNghiepIdAsync(Guid doanhNghiepId)
        {
            return await _context.SanPhams
                .Include(x => x.MaQrSanPhams)
                .Where(x => x.DoanhNghiepId == doanhNghiepId && !x.XoaMem)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        public Task<List<SanPham>> GetAllAsync()
        {
            
            return _context.SanPhams
                .Where(sp => !sp.XoaMem)   
                .ToListAsync();
        }
        public async Task<SanPham> AddAsync(SanPham sanPham)
        {
            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }

        public async Task<SanPham?> UpdateAsync(SanPham sanPham)
        {
            _context.SanPhams.Update(sanPham);
            await _context.SaveChangesAsync();
            return sanPham;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var entity = await _context.SanPhams
                .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);

            if (entity == null) return false;

            entity.XoaMem = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<SanPhamSearchResultDto>> SearchAsync(string keyword)
        {
            return await _context.SanPhams
                .Where(x => !x.XoaMem &&
                    (
                        x.Ten.Contains(keyword) ||
                        (x.MaSanPham != null && x.MaSanPham.Contains(keyword))
                    )
                )
                .Include(x => x.DoanhNghiep)
                .OrderBy(x => x.Ten)
                .Select(x => new SanPhamSearchResultDto
                {
                    Id = x.Id,
                    TenSanPham = x.Ten,
                    Gia = x.Gia,
                    SoLuong = x.SoLuong,
                    HinhAnhUrl = x.HinhAnhUrl,
                    TenDoanhNghiep = x.DoanhNghiep.Ten
                })
                .ToListAsync();
        }


    }

}
