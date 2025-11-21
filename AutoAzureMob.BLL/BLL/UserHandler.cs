using AutoAzureMob.Core.Email;
using AutoAzureMob.Core.JWTToken;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.DashBoardDTO;
using AutoAzureMob.Models.DTO.DeviceToken;
using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.Company;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.User;
using AutoAzureMob.Models.VM.DashBoard;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.BLL
{
    public class UserHandler  :BaseHandler
    {
        private readonly UserDAO userDAO;
        private readonly NotificationDAO notificationDAO;
        private readonly DashBoardDAO dashBoardDAO;
        private readonly DashBoardHandler dashBoardHandler;
        private readonly IConfiguration config;
        public UserHandler(ExecuteContext executeContext, IConfiguration _config): base(executeContext,_config) 
        {
            config = _config;
            userDAO = new UserDAO(executeContext, config);
            notificationDAO = new NotificationDAO(executeContext, config);
            dashBoardDAO = new DashBoardDAO(executeContext, config);
            dashBoardHandler = new DashBoardHandler(executeContext, config);    
        }
        public ResponseModel<string> ValidateIfUserActivated(int userId)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Success = true;
            response.Content ="success";
            return response;
        }
        public ResponseModel<LoginResponseDTO> Login(LoginRequest login)
        {
            ResponseModel<LoginResponseDTO> response = new ResponseModel<LoginResponseDTO>();
            UserInfo userInfo = userDAO.validateCredentials(login);
            bool LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
            if (!string.IsNullOrEmpty(userInfo.Email))
            {
                string functionKey =LiveServer ? config.GetSection("App-Keys:Prod-Key").Value : config.GetSection("App-Keys:Key").Value;
                AccountsDTO accounts = new AccountsDTO() { ChannelId = userInfo.UserId };
                response.Content = new LoginResponseDTO()
                {
                    Token = TokenGenerator.GetToken(userInfo, config),
                    Key = functionKey,
                    IsFresh = dashBoardDAO.GetLinkedAccounts(accounts).Count() > 0 ? false: true,
                    UserInfo = userInfo
                };

                response.Success = true;
                response.Title = "Inicio de sesión exitoso";
                response.Description = "Inicio de sesión de usuario.";
            }
            else
            {
                response.Success=false;
                response.Title = "Error credenciales incluidas.";
                response.Description = "El nombre de la empresa o la contraseña no son válidos.";    
            }
           
            return response;
        }
        public ResponseModel<LoginResponseVM> LoginV2(LoginRequest login)
        {
            ResponseModel<LoginResponseVM> response = new ResponseModel<LoginResponseVM>();
            response.Content = new LoginResponseVM();
            UserInfo userInfo = userDAO.validateCredentials(login);
            bool LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
            if (!string.IsNullOrEmpty(userInfo.Email))
            {
                string functionKey = LiveServer ? config.GetSection("App-Keys:Prod-Key").Value : config.GetSection("App-Keys:Key").Value;
                AccountsDTO accounts = new AccountsDTO() { CompanyId = userInfo.CompanyId };
                var linkedAccounts = dashBoardHandler.GetLinkedAccountsList(accounts);
                response.Content.LinkedAccounts = linkedAccounts.Content;
                response.Content.LoginResponseDTO = new LoginResponseDTO()
                {
                    Token = TokenGenerator.GetToken(userInfo, config),
                    Key = functionKey,
                    IsFresh = linkedAccounts.Content.Count() > 0 ? false : true,
                    UserInfo = userInfo
                };
                response.Success = true;
                response.Title = "Inicio de sesión exitoso";
                response.Description = "Inicio de sesión de usuario.";
            }
            else
            {
                response.Success = false;
                response.Title = "Error credenciales incluidas.";
                response.Description = "El nombre de la empresa o la contraseña no son válidos.";
            }

            return response;
        }
        public ResponseModel<string> AdminCompanyRegistration(RegistRequest reg)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            RegistResponse regist = userDAO.AdminCompanyRegistration(reg);
            if (regist.Result.ToLower().Equals("ok"))
            {
                var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string template = File.ReadAllText(binDirectory + "/Templates/RegistrationTemplate.txt");
                template = template.Replace("{CompanyName}", regist.CompanyCode);
                template = template.Replace("{UserName}", regist.UserName);
                template = template.Replace("{Phone}", reg.MainPhone);
                template = template.Replace("{Email}", reg.MainEmail);
                template = template.Replace("{Role}", regist.RoleName);
                template = template.Replace("{Password}", regist.Password);
                EmailServices.SendEmail(config, template, reg.MainEmail, "Registrarte");
                response.Success = true;
                response.Title = "Registro exitoso.";
                response.Description = " Por favor, visite su correo para obtener los detalles de inicio de sesión.";
            }
            else
            {
                response.Success = false;
                response.Description = regist.ResultText;
                response.Title = "Registro fallido";
            }
            return response;
        }
        public ResponseModel<string> ForgetPasswordRequest(ForgetPassDTO forget)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            ForgetPassResponse result = userDAO.ForgetPassRequest(forget);
            if (result.Result.ToLower().Equals("ok"))
            {

                string link = config.GetSection("URLs:ForgetPasswordUrl").Value + "?ID=" + result.Token.ToString();
                var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string template = File.ReadAllText(binDirectory + "/Templates/ForgetPasswordTemplate.txt");
                template = template.Replace("{MainContact}", result.MainContact);
                template = template.Replace("{CompanyName}", forget.CompanyCode);
                template = template.Replace("{UserName}", forget.UserName);
                template = template.Replace("{ForgetLink}", link);
                EmailServices.SendEmail(config, template, result.Email, "Contraseña olvidada");
                response.Success = true;
                response.Title = "¡Email enviado!";
                response.Description = " Recibirá un correo electrónico para restablecer su contraseña.";
            }
            else
            {
                response.Success = false;
                response.Title = "error!";
                response.Description = "el código de empresa o el nombre de usuario no es válido";
            }
            return response;
        }
        public ResponseModel<string> SaveUserDeviceToken(DeviceTokenDTO device)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = notificationDAO.SaveUserDeviceToken(device);
            response.Success = true;
            response.Description = "Device Token updated succssfuly";
            return response;
        }
    }
}
