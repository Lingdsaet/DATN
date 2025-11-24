namespace DATN.ReponseDto
{
    public class AuthResponseDto<TUser>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }      // sau này bạn gắn JWT vào
        public TUser? User { get; set; }
    }
}
