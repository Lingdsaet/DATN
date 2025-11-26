using DATN.Model;
using DATN.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<QR_DATNContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Repository
builder.Services.AddScoped<INguoiTieuDungRepository, NguoiTieuDungRepository>();
builder.Services.AddScoped<ILichSuQuetRepository, LichSuQuetRepository>();
builder.Services.AddScoped<ILoHangRepository, LoHangRepository>();
builder.Services.AddScoped<IMaQrLoHangRepository, MaQrLoHangRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<ICuaHangRepository, CuaHangRepository>();
builder.Services.AddScoped<IDoanhNghiepRepository, DoanhNghiepRepository>();
builder.Services.AddScoped<IYeuCauDangKyDnRepository, YeuCauDangKyDnRepository>();
builder.Services.AddScoped<IQrScanRepository, QrScanRepository>();


//Đăng ký Controllers 
builder.Services.AddControllers();

// Đăng ký Authorization (để dùng được app.UseAuthorization)
builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Nếu có dùng HTTPS:
app.UseHttpsRedirection();

// Nếu sau này có Authentication thì:
// app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
