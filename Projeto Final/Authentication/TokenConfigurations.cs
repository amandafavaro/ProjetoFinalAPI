namespace Projeto_Final.Authentication
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public int ExpirationTimeInHours { get; set; }
    }
}
