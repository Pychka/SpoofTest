using DataTransferObjects;
using Microsoft.IdentityModel.Tokens;
using SpoofTestApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpoofTestApi.Services;

public class TokenCreator<T> where T : BaseEntity
{
    private static string GetJWT(T entity)
    {
        DateTime now = DateTime.UtcNow;
        List<Claim> claims = [new Claim("UserId", entity.Id.ToString())];

        JwtSecurityToken jwt = new(
            issuer: AuthOptions.ISSUER,
            audience: typeof(T).Name,
            notBefore: now,
            claims: new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType).Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
        string? encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
