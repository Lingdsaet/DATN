using System;

namespace DATN.ReponseDto
{
    public class LichSuQuetItemDto
    {
        public Guid Id { get; set; }               // Id lần quét
        public string MaQR { get; set; } = null!;
        public DateTime ThoiGian { get; set; }
        public string KetQua { get; set; } = null!;
       
    }
}
