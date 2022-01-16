namespace OneWorld.DTO.Response
{
    public class AccountLoginResponse : BaseErrorSuccess
    {
        public AccountLoginResponse()
        {
        }

        public AccountLoginResponse(bool success)
        {
            Success = success;
        }

        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}