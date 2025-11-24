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

    public virtual DbSet<NguoiTieuDung> NguoiTieuDungs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<YeuCauDangKyDn> YeuCauDangKyDns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = DESKTOP-J4NABFA ; Database =QR_DATN;User ID=sa;Password=1234;Encrypt=false;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoNguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BaoCaoNg__3214EC0764584E66");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.LanQuet).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_LanQuet");

            entity.HasOne(d => d.MaQrLoHang).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_QR");

            entity.HasOne(d => d.NguoiTieuDung).WithMany(p => p.BaoCaoNguoiDungs).HasConstraintName("FK_BCND_NTD");
        });

        modelBuilder.Entity<CuaHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CuaHang__3214EC07557150F6");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DiaDiem).WithMany(p => p.CuaHangs).HasConstraintName("FK_CuaHang_DiaDiem");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.CuaHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CuaHang_DoanhNghiep");
        });

        modelBuilder.Entity<DiaDiem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DiaDiem__3214EC073C7A8FD2");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<DmLoaiSuKien>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_LoaiS__3214CC9FC35A9965");
        });

        modelBuilder.Entity<DmTrangThaiQr>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_Trang__3214CC9FBBFD3300");
        });

        modelBuilder.Entity<DoanhNghiep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DoanhNgh__3214EC07BE295F18");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<LichSuQuet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LanQuet__3214EC07F1DD02E2");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.MaQrLoHang).WithMany(p => p.LichSuQuets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LanQuet_QR");

            entity.HasOne(d => d.NguoiTieuDung).WithMany(p => p.LichSuQuets).HasConstraintName("FK_LanQuet_NTD");
        });

        modelBuilder.Entity<LoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoHang__3214EC07CB649EB1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.SanPham).WithMany(p => p.LoHangs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoHang_SanPham");
        });

        modelBuilder.Entity<MaQrLoHang>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaQR_LoH__3214EC076A7032A1");

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
            entity.HasKey(e => e.Id).HasName("PK__NguoiDun__3214EC07BCBD19B2");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.NguoiDungs).HasConstraintName("FK_NguoiDung_DoanhNghiep");

            entity.HasMany(d => d.VaiTros).WithMany(p => p.NguoiDungs)
                .UsingEntity<Dictionary<string, object>>(
                    "NguoiDungVaiTro",
                    r => r.HasOne<VaiTro>().WithMany()
                        .HasForeignKey("VaiTroId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NDV_VaiTro"),
                    l => l.HasOne<NguoiDung>().WithMany()
                        .HasForeignKey("NguoiDungId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_NDV_NguoiDung"),
                    j =>
                    {
                        j.HasKey("NguoiDungId", "VaiTroId").HasName("PK__NguoiDun__B0CCFCAC00B6E4F7");
                        j.ToTable("NguoiDung_VaiTro");
                    });
        });

        modelBuilder.Entity<NguoiTieuDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NguoiTie__3214EC073F050A7B");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC07D20E0DD1");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.SanPhams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_DoanhNghiep");
        });

        modelBuilder.Entity<SuKienChuoiCungUng>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SuKienCh__3214EC07FE7C8755");

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
            entity.HasKey(e => e.Id).HasName("PK__VaiTro__3214EC078356F312");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<YeuCauDangKyDn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__YeuCauDa__3214EC07FBB32C56");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
