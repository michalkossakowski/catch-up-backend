namespace catch_up_backend.Dtos
{
    public class AuthResponseDto{
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }


        public AuthResponseDto(string accesToken, string refreshToken)
        {
            this.AccessToken = accesToken;
            this.RefreshToken = refreshToken;
        }
    }
}
