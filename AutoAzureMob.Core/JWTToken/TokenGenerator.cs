using AutoAzureMob.Models.Models.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.Core.JWTToken
{
    public static class TokenGenerator
    {
        public static string GetToken(UserInfo user, IConfiguration config)
        {
            var claims = new List<Claim>
                {
                    new Claim("userid", user.UserId.ToString()),
                    new Claim("companyId", user.CompanyId.ToString()),
                    new Claim("emails", user.Email),
                    new Claim("token", user.Token),
                    new Claim("isadmin", user.IsAdmin.ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(365)).ToUnixTimeSeconds().ToString())
                };

            var key = config.GetSection("JwtToken:Key").Value.ToString();
            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
