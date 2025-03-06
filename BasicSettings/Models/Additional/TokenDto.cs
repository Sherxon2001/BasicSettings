namespace BasicSettings.Models.Additional
{
    public class TokenDto
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long AuthDateUnix => ((DateTimeOffset)ExpirationDate).ToUnixTimeSeconds();
        public ApplicantUserDto User { get; set; }
    }
}
