namespace OneWorld.DTO
{
    public class AccountRegisterResult : BaseErrorSuccess
    {
        public AccountRegisterResult()
        {
        }

        public AccountRegisterResult(bool success)
        {
            Success = success;
        }

        public string EmailVerificationUrl { get; set; }
        
    }
}