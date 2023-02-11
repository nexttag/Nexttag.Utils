namespace Nexttag.Authentication.Jwt
{
    /// <summary>
    /// Default values related to basic authentication middleware
    /// </summary>
    public class JwtAuthenticationScheme : IAuthenticationScheme
    {
        public static string AuthenticationScheme = "Bearer";

        /// <summary>
        /// The default value used for BasicAuthenticationOptions.AuthenticationScheme
        /// </summary>
        public string Scheme { get => AuthenticationScheme; set { } }
    }
}
