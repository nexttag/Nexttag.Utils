using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Nexttag.Utils.Authentication.Jwt
{
    public class JwtSecurityTokenResolver
    {
        private readonly JWTTokenConfiguration _jwtTokenConfiguration;
        private IEnumerable<Claim> _claims;

        public JwtSecurityTokenResolver(JWTTokenConfiguration jwtTokenConfiguration)
        {
            _jwtTokenConfiguration = jwtTokenConfiguration;
        }

        public void AddClaims(IEnumerable<Claim> claims)
        {
            _claims = claims;
        }

        public string ToRefreshTokenString(IEnumerable<Claim> claims = null, int? overrideExpireMinute = null)
        {
            return ToTokenString(claims, overrideExpireMinute ?? _jwtTokenConfiguration.RefreshTokenExpireInMinutes);
        }

        public string ToRefreshTokenString(JwtPayload payload, int? overrideExpireMinute = null)
        {
            return ToTokenString(payload, overrideExpireMinute ?? _jwtTokenConfiguration.RefreshTokenExpireInMinutes);
        }

        public string ToTokenString(IEnumerable<Claim> claims = null, int? overrideExpireMinute = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                claims: claims ?? _claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(overrideExpireMinute ?? _jwtTokenConfiguration.TokenExpireInMinutes),
                signingCredentials: creds
            );
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        public bool ValidateToken(string refreshToken)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key));
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _jwtTokenConfiguration.Issuer,
                    ValidAudience = _jwtTokenConfiguration.Audience,
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public DateTime GetExpiredDate(string refreshToken)
        {
            var secondsToExpired = Convert.ToDouble(GetClaim(refreshToken, JwtRegisteredClaimNames.Exp));
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(secondsToExpired);
        }

        public string ToTokenString(JwtPayload payload, int? overrideExpireMinute = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenConfiguration.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtTokenConfiguration.Issuer,
                audience: _jwtTokenConfiguration.Audience,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(overrideExpireMinute ?? _jwtTokenConfiguration.TokenExpireInMinutes),
                signingCredentials: creds
            );

            if (_claims != null && _claims.Any())
            {
                token.Payload.AddClaims(_claims);
            }

            foreach (var claim in payload.Keys)
            {
                token.Payload.Add(claim, payload[claim]);
            }

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }


        public string GetClaim(string token, string claim)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var claims = jsonToken as JwtSecurityToken;
            var claimValue = claims.Claims.FirstOrDefault(c => c.Type == claim)?.Value ?? "";
            return claimValue;
        }
    }
}