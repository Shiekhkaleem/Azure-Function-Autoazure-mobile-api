using AutoAzureMob.API.ExceptionHandling;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DeviceToken;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.API.ActionFilter
{
    public class JWTMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly UserHandler _userHandler;
        private readonly IConfiguration _configuration;
        private readonly ExecuteContext executecontext;
        public JWTMiddleware(ExecuteContext _executecontext= null)
        {
            _configuration = ConfigurationHelper.GetConfiguration();
            executecontext = _executecontext;
            _userHandler = new UserHandler(executecontext, _configuration);
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        
        {
            var req = context.GetHttpRequestData();
            var headers = req.Headers;
            headers.TryGetValues("Authorization", out var authHeader);
            headers.TryGetValues("DeviceToken", out var device_token);
            string deviceToken = string.Empty;
            string deviceType = string.Empty;
            if (headers.Contains("DeviceToken"))
            {
               string tokenString = !string.IsNullOrEmpty(device_token?.FirstOrDefault()?.ToString()) ? device_token?.FirstOrDefault()?.ToString() : "";
                deviceToken = tokenString.Split(' ').LastOrDefault();
                deviceType = tokenString.Split(' ').FirstOrDefault();
            }

            if (authHeader != null)
            {
                var token = authHeader?.FirstOrDefault()?.ToString();

                if (!string.IsNullOrEmpty(token))
                    attachAccountToContext(context, token, deviceToken, deviceType);
            }
            await next(context);
        }
        private void attachAccountToContext(FunctionContext context, string token, string deviceToken = "", string deviceType = "",HttpResponseData response = null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration.GetValue<string>("JwtToken:Key");

              var claimsPrinciple  = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(365)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "userid").Value;
                var exist = claimsPrinciple.FindFirst("companyId");
                int companyId = default;
                if (exist != null)
                {
                    companyId = Convert.ToInt32(jwtToken.Claims.First(x => x.Type == "companyId").Value);
                }

                // attach account to context on successful jwt validation
                context.Items["userId"] = userId;
                context.Items["User"] = !string.IsNullOrWhiteSpace(userId) ? _userHandler.ValidateIfUserActivated(Convert.ToInt32(userId)) : null;
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    DeviceTokenDTO device = new DeviceTokenDTO()
                    {
                        UserId = companyId,
                        DeviceToken = deviceToken,
                        Type = deviceType,
                    };
                    _userHandler.SaveUserDeviceToken(device);
                }
            }
            catch (SecurityTokenExpiredException)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.WriteAsJsonAsync("Token expired");
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.WriteAsJsonAsync("Invalid token signature");
            }
            catch (SecurityTokenValidationException)
            {
                response.StatusCode = HttpStatusCode.Unauthorized;
                response.WriteAsJsonAsync("Invalid token");
            }
        }
    }
}
