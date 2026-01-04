namespace DATN.RequestDto
{
    public class TinTucCreateUpdateDto
    {
        public string TieuDe { get; set; } = null!;
        public string? TomTat { get; set; }
        public string NoiDung { get; set; } = null!;
        public string? HinhAnhUrl { get; set; }
        public bool NoiBat { get; set; }
    }

    public class TinTucHomeDto
    {
        public Guid Id { get; set; }
        public string TieuDe { get; set; } = null!;
        public string? TomTat { get; set; }
        public string? HinhAnhUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TinTucDetailDto
    {
        public Guid Id { get; set; }
        public string TieuDe { get; set; } = null!;
        public string? TomTat { get; set; }
        public string NoiDung { get; set; } = null!;
        public string? HinhAnhUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
