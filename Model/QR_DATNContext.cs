using DATN.Model;
using DATN.Model.DATN.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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

    public virtual DbSet<DoanhNghiep> DoanhNghieps { get; set; }

    public virtual DbSet<LichSuQuet> LichSuQuets { get; set; }

    public virtual DbSet<LoHang> LoHangs { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<LoaiSuKien> LoaiSuKiens { get; set; }

    public virtual DbSet<MaQrLoHang> MaQrLoHangs { get; set; }

    public virtual DbSet<MaQrSanPham> MaQrSanPhams { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }

    public virtual DbSet<SuKienChuoiCungUng> SuKienChuoiCungUngs { get; set; }

    public virtual DbSet<TrangThaiQr> TrangThaiQrs { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<YeuCauDangKyDn> YeuCauDangKyDns { get; set; }

    public virtual DbSet<NguoiDungVaiTro> NguoiDungVaiTro { get; set; }
    public virtual DbSet<TinTuc> TinTucs { get; set; }


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

        modelBuilder.Entity<DoanhNghiep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DoanhNgh__3214EC0761E1D8B3");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<LichSuQuet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuQu__3214EC07B4D69953");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.MaQrLoHang).WithMany(p => p.LichSuQuets).HasConstraintName("FK_LanQuet_QR");

            entity.HasOne(d => d.MaQrSanPham).WithMany(p => p.LichSuQuets).HasConstraintName("FK_LichSuQuet_MaQR_SanPham");

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

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiSanP__3214EC07A5A1EC29");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
        });

        modelBuilder.Entity<LoaiSuKien>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_LoaiS__3214CC9F71D7CF50");
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

        modelBuilder.Entity<MaQrSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaQR_San__3214EC070F5ED3DC");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.SanPham).WithMany(p => p.MaQrSanPhams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoHangImage_LoHang");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NguoiDun__3214EC07AC31B2CF");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.NguoiDungs).HasConstraintName("FK_NguoiDung_DoanhNghiep");

            modelBuilder.Entity<NguoiDungVaiTro>(entity =>
            {
                entity.ToTable("NguoiDungVaiTro");

                entity.HasKey(x => new { x.NguoiDungId, x.VaiTroId });

                entity.HasOne(x => x.NguoiDung)
                    .WithMany(u => u.NguoiDungVaiTros)
                    .HasForeignKey(x => x.NguoiDungId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NDV_NguoiDung");

                entity.HasOne(x => x.VaiTro)
                    .WithMany(r => r.NguoiDungVaiTros)
                    .HasForeignKey(x => x.VaiTroId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NDV_VaiTro");
            });

        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC0788CCB47A");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.DoanhNghiep).WithMany(p => p.SanPhams)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SanPham_DoanhNghiep");

            entity.HasOne(d => d.LoaiSanPham).WithMany(p => p.SanPhams).HasConstraintName("FK_SanPham_LoaiSanPham");
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

        modelBuilder.Entity<TrangThaiQr>(entity =>
        {
            entity.HasKey(e => e.Ma).HasName("PK__DM_Trang__3214CC9F24CDCCA5");
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

        modelBuilder.Entity<DanhGiaSanPham>(entity =>
        {
            entity.ToTable("DanhGiaSanPham", t =>
            {
                t.HasCheckConstraint(
                    "CK_DanhGiaSanPham_SoSao",
                    "[SoSao] BETWEEN 1 AND 5"
                );
            });

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newsequentialid())");

            entity.Property(e => e.SoSao)
                .IsRequired();

            entity.Property(e => e.NoiDung)
                .HasMaxLength(1000);

            entity.Property(e => e.CreatedAt)
                .HasPrecision(0);

            entity.Property(e => e.UpdatedAt)
                .HasPrecision(0);

            entity.Property(e => e.XoaMem)
                .HasDefaultValue(false);

            entity.HasOne(d => d.SanPham)
                .WithMany(p => p.DanhGiaSanPhams)
                .HasForeignKey(d => d.SanPhamId);

            entity.HasOne(d => d.NguoiDung)
                .WithMany(p => p.DanhGiaSanPhams)
                .HasForeignKey(d => d.NguoiDungId);
        });

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
