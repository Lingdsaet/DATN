namespace DATN.ReponseDto
{
    public class DiaDiemResponseDto
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = null!;
        public string? DiaChiDayDu { get; set; }
        public decimal? ViDo { get; set; }
        public decimal? KinhDo { get; set; }
    }
}
