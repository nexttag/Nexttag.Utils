namespace Nexttag.Utils.Authentication.Jwt
{
    public class JWTTokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
        public int TokenExpireInMinutes { get; set; }
        public int RefreshTokenExpireInMinutes { get; set; }

    }
}