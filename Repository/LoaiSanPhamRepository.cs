using DATN.Model;
using DATN.Repository;
using Microsoft.EntityFrameworkCore;

namespace DATN.Repository
{ 
public class LoaiSanPhamRepository : ILoaiSanPhamRepository
{
    private readonly QR_DATNContext _context;

    public LoaiSanPhamRepository(QR_DATNContext context)
    {
        _context = context;
    }

    public async Task<List<LoaiSanPham>> GetAllAsync()
    {
        return await _context.LoaiSanPhams
            .Where(x => !x.XoaMem)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<LoaiSanPham?> GetByIdAsync(Guid id)
    {
        return await _context.LoaiSanPhams
            .FirstOrDefaultAsync(x => x.Id == id && !x.XoaMem);
    }

    public async Task<bool> ExistsByMaLoaiAsync(string maLoai)
    {
        return await _context.LoaiSanPhams
            .AnyAsync(x => x.MaLoai == maLoai && !x.XoaMem);
    }

    public async Task AddAsync(LoaiSanPham entity)
    {
        await _context.LoaiSanPhams.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(LoaiSanPham entity)
    {
        _context.LoaiSanPhams.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> SoftDeleteAsync(Guid id)
    {
        var entity = await _context.LoaiSanPhams.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return false;

        entity.XoaMem = true;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }
}

    
}
