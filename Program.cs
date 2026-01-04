using DATN.Model;
using DATN.Repository;
using DATN.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpClient();

//  Named client cho Chat API
builder.Services.AddHttpClient("gemini", (sp, c) =>
{
    var cfg = sp.GetRequiredService<IConfiguration>();

    c.BaseAddress = new Uri(cfg["Gemini:BaseUrl"]!);
    c.Timeout = TimeSpan.FromSeconds(60);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});


const string AllowFrontend = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});


// Đăng ký DbContext
builder.Services.AddDbContext<QR_DATNContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Repository

builder.Services.AddScoped<ILichSuQuetRepository, LichSuQuetRepository>();
builder.Services.AddScoped<ILoHangRepository, LoHangRepository>();
builder.Services.AddScoped<IMaQrLoHangRepository, MaQrLoHangRepository>();
builder.Services.AddScoped<ISanPhamRepository, SanPhamRepository>();
builder.Services.AddScoped<ICuaHangRepository, CuaHangRepository>();
builder.Services.AddScoped<IDoanhNghiepRepository, DoanhNghiepRepository>();
builder.Services.AddScoped<IYeuCauDangKyDnRepository, YeuCauDangKyDnRepository>();
builder.Services.AddScoped<IQrScanRepository, QrScanRepository>();
builder.Services.AddScoped<INguoiDungRepository, NguoiDungRepository>();
builder.Services.AddScoped<IVaiTroRepository, VaiTroRepository>();
builder.Services.AddScoped<INguoiDungVaiTroRepository, NguoiDungVaiTroRepository>();
builder.Services.AddScoped<IFirebaseService, FirebaseService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<IMaQrSanPhamRepository, MaQrSanPhamRepository>();
builder.Services.AddScoped<ILoaiSanPhamRepository, LoaiSanPhamRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IDanhGiaSanPhamRepository, DanhGiaSanPhamRepository>();
//builder.Services.AddScoped<ISuKienChuoiCungUngRepository, SuKienChuoiCungUngRepository>();
//builder.Services.AddScoped<IDmLoaiSuKienRepository, DmLoaiSuKienRepository>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50 * 1024 * 1024;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
});

//Đăng ký Controllers 
builder.Services.AddControllers();

// 2. Đăng ký service tạo token
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// 3. Thêm Authentication + JWT Bearer
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = jwtSection.GetValue<string>("Key");
var issuer = jwtSection.GetValue<string>("Issuer");
var audience = jwtSection.GetValue<string>("Audience");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key!))
        };
    });

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
//app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
