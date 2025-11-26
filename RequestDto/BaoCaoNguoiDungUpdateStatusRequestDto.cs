namespace DATN.RequestDto
{
    public class BaoCaoNguoiDungUpdateStatusRequestDto
    {
        // OPEN / IN_PROGRESS / RESOLVED / REJECTED
        public string TrangThaiXuLy { get; set; } = null!;
        public string? GhiChuXuLy { get; set; }
    }
}
