namespace Broker.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }
        public LoginType? LoginType { get; set; }
    }

    public class GetIsEmailExist
    {
        public bool Exist { get; set; }
        public string Error { get; set; }
        public bool Success { get; set; }
    }
    public enum LoginType
    {
        seeker = 0,
        owner = 1,
        broker = 2,
        company = 3

    }
}
