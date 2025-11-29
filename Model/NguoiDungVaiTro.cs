namespace DATN.Model
{
    public class NguoiDungVaiTro
    {
        public Guid NguoiDungId { get; set; }
        public byte VaiTroId { get; set; }  // TINYINT -> byte

        // Navigation
        public NguoiDung NguoiDung { get; set; } = null!;
        public VaiTro VaiTro { get; set; } = null!;
    }

}
