using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DATN.Model;

public partial class QR_DATNContext : DbContext
{
    public QR_DATNContext()
    {
    }

    public QR_DATNContext(DbContextOptions<QR_DATNContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaoCaoNguoiDung> BaoCaoNguoiDungs { get; set; }

    public virtual DbSet<CuaHang> CuaHangs { get; set; }

    public virtual DbSet<DiaDiem> DiaDiems { get; set; }

    public virtual DbSet<DmLoaiSuKien> DmLoaiSuKiens { get; set; }

    public virtual DbSet<DmTrangThaiQr> DmTrangThaiQrs { get; set; }

    public virtual DbSet<DoanhNghiep> DoanhNghieps { get; set; }

    public virtual DbSet<LichSuQuet> LichSuQuets { get; set; }

    public virtual DbSet<LoHang> LoHangs { get; set; }

    public virtual DbSet<MaQrLoHang> MaQrLoHangs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<YeuCauDangKyDn> YeuCauDangKyDns { get; set; }

    public DbSet<NguoiDungVaiTro> NguoiDungVaiTro { get; set; } = null!;
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = DESKTOP-J4NABFA ; Database =QR_DATN;User ID=sa;Password=1234;Encrypt=false;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoNguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCaoNg__3214EC079DD83DAF");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.LanQuet).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_LanQuet");

            entity.HasOne(d => d.MaQrLoHang).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_QR");

            entity.HasOne(d => d.NguoiDung).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_User");
        });

        modelBuilder.Entity<NguoiDungVaiTro>(entity =>
        {
            entity.ToTable("NguoiDung_VaiTro");          // tên bảng trong SQL

            entity.HasKey(x => new { x.NguoiDungId, x.VaiTroId });

            entity.HasOne(x => x.NguoiDung)
                  .WithMany()                            // hoặc WithMany(x => x.NguoiDungVaiTros) nếu bạn có collection
                  .HasForeignKey(x => x.NguoiDungId);

            entity.HasOne(x => x.VaiTro)
                  .WithMany()
                  .HasForeignKey(x => x.VaiTroId);
        });

        modelBuilder.Entity<CuaHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CuaHang__3214EC0786DB7C70");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DiaDiem).WithMany(p => p.CuaHangs).HasConstraintName("FK_CuaHang_DiaDiem");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.CuaHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CuaHang_DoanhNghiep");
        });

        modelBuilder.Entity<DiaDiem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DiaDiem__3214EC079112EF75");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<DmLoaiSuKien>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_LoaiS__3214CC9F71D7CF50");
        });

        modelBuilder.Entity<DmTrangThaiQr>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_Trang__3214CC9F24CDCCA5");
        });

        modelBuilder.Entity<DoanhNghiep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DoanhNgh__3214EC0761E1D8B3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<LichSuQuet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuQu__3214EC07B4D69953");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.MaQrLoHang).WithMany(p => p.LichSuQuets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanQuet_QR");

            entity.HasOne(d => d.NguoiDung).WithMany(p => p.LichSuQuets).HasConstraintName("FK_LanQuet_User");
        });

        modelBuilder.Entity<LoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoHang__3214EC07A9F26D12");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.SanPham).WithMany(p => p.LoHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoHang_SanPham");
        });

        modelBuilder.Entity<MaQrLoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaQR_LoH__3214EC0711BAE841");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.LoHang).WithMany(p => p.MaQrLoHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaQR_LoHang");

            entity.HasOne(d => d.TrangThaiNavigation).WithMany(p => p.MaQrLoHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MaQR_TrangThai");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NguoiDun__3214EC07AC31B2CF");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.NguoiDungs).HasConstraintName("FK_NguoiDung_DoanhNghiep");

            //entity.HasMany(d => d.VaiTros).WithMany(p => p.NguoiDungs)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "NguoiDungVaiTro",
            //        r => r.HasOne<VaiTro>().WithMany()
            //            .HasForeignKey("VaiTroId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_NDV_VaiTro"),
            //        l => l.HasOne<NguoiDung>().WithMany()
            //            .HasForeignKey("NguoiDungId")
            //            .OnDelete(DeleteBehavior.ClientSetNull)
            //            .HasConstraintName("FK_NDV_NguoiDung"),
            //        j =>
            //        {
            //            j.HasKey("NguoiDungId", "VaiTroId").HasName("PK__NguoiDun__B0CCFCAC0CA4532C");
            //            j.ToTable("NguoiDung_VaiTro");
            //        });
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC0788CCB47A");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.SanPhams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_DoanhNghiep");
        });

        modelBuilder.Entity<SuKienChuoiCungUng>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SuKienCh__3214EC0740D5C7F3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DiaDiem).WithMany(p => p.SuKienChuoiCungUngs).HasConstraintName("FK_SKCCU_DiaDiem");

            entity.HasOne(d => d.LoHang).WithMany(p => p.SuKienChuoiCungUngs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SKCCU_LoHang");

            entity.HasOne(d => d.LoaiSuKienNavigation).WithMany(p => p.SuKienChuoiCungUngs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SKCCU_Loai");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__VaiTro__3214EC07926265D9");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<YeuCauDangKyDn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__YeuCauDa__3214EC0709C42057");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
