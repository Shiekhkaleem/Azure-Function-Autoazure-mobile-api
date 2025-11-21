using AutoAzureMob.BLL.Utils;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.FacturacionDTO;
using AutoAzureMob.Models.Enums;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Facturacion;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.Sale;
using AutoAzureMob.Models.Models.User;
using AutoAzureMob.Models.VM.Facturacion;
using AutoAzureMob.Models.VM.Notification;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using AzureUrl = AutoAzureMob.QueryResource.AzureUrls;
using AzureUrlProd = AutoAzureMob.QueryResource.AzureUrlsProd;

namespace AutoAzureMob.BLL.BLL
{
    public class ConfiguracionHandler : BaseHandler
    {
        private readonly IConfiguration _config;
        private readonly ConfiguracionDAO configuracionDAO;
        private static bool LiveServer = false;
        public ConfiguracionHandler(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
            configuracionDAO = new ConfiguracionDAO(executeContext, config);
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
        }
        #region Facturacion
        #region Get Profiles
        public ResponseModel<List<Profile>> GetPerfileList(int companyId, int page)
        {
            ResponseModel<List<Profile>> response = new ResponseModel<List<Profile>>();
            response.Content = configuracionDAO.GetProfileList(companyId,page);
            response.Description = "Perfiles no encontrados.";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Lista de perfiles.";
                response.Success = true;
            }
            return response;
        }
        #endregion

        #region Get User Relacion List
        public ResponseModel<UserRelacionVM> GetUserRelacionList(int companyId)
        {
            ResponseModel<UserRelacionVM> response = new ResponseModel<UserRelacionVM>();
            response.Content = new();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content.Relacion = configuracionDAO.GetUserRelacionList(companyId);
            response.Content.RelacionField = configuracionDAO.GetRelacionFieldList(companyId);
            response.Title = "Failed.";
            response.Description = "Relation and fields not found.";
            if (response.Content.Relacion.Count > 0)
            {
                response.Content.Relacion.ForEach(e =>
                {
                    e.ChannelThumbnail = logos.FirstOrDefault(x => x.Id == e.ChannelId).Url;
                });
                response.Description = "Relation and fields list.";
                response.Title = "Success";
                response.Success = true;
            }
            return response;
        }
        #endregion
        #region Get Timbers List and Perfile
        public ResponseModel<TimberVM> GetTimberPerfileList(int companyId)
        {
            ResponseModel<TimberVM> response = new ResponseModel<TimberVM>();
            response.Content = new TimberVM();
            response.Content.TimberPerfileList = configuracionDAO.GetTimberProfileList(companyId);
            response.Content.TimberList = configuracionDAO.GetTimberList();
            response.Description = "Lista de perfiles y maderas no encontrada.";
            if (response.Content != null && response.Content.TimberPerfileList.Count > 0)
            {
                response.Description = "Lista de perfiles y maderas.";
                response.Success = true;
            }
            return response;
        }

        public ResponseModel<TimberQuantity> GetTimberQuantity(string companyId, string profileId)
        {
            ResponseModel<TimberQuantity> response = new ResponseModel<TimberQuantity>();
            // string url = LiveServer ? AzureUrlProd.GetAvailableQuantity : AzureUrl.GetAvailableQuantity;
            string url = AzureUrlProd.GetAvailableQuantity;
            string json = JsonConvert.SerializeObject(new
            {
                CompanyID = companyId,
                ProfileID = profileId,
            });
            response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<TimberQuantity>>(url, json);
            if (response == null)
            {
                response = new();
                response.Title = "fallida";
                response.Success = false;
                response.Description = "cantidad de madera no encontrada";
            }
            return response;
        }
        #endregion

        #region Get Configuracion Setting and FormadePago List
        public ResponseModel<ConfiguracionVM> GetConfigFormadePagoSetting(int companyId)
        {
            ResponseModel<ConfiguracionVM> response = new ResponseModel<ConfiguracionVM>();
            response.Content = new ConfiguracionVM();
            response.Content.ConfiguracionSetting = configuracionDAO.GetConfiguracionSetting(companyId);
            response.Content.ConfigFormadePagoList = configuracionDAO.GetConfigFormadePagoList();
            response.Description = "Ajustes de configuración no encontrados.";
            if (response.Content != null && response.Content.ConfigFormadePagoList.Count > 0)
            {
                response.Description = "Ajustes de configuración.";
                response.Success = true;
            }
            return response;
        }
        #endregion

        #region Save User Profile Relation
        public ResponseModel<string> SaveUserProfileRelation(List<SaveUserDTO> req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            List<RelationRequest> requestList = new();
            string url = LiveServer ? AzureUrlProd.SaveUserProfileRelation : AzureUrl.SaveUserProfileRelation;
            if (req.Count > 0)
            {
                var groupByList = req.GroupBy(x => x.ChannelId);
                foreach (var item in groupByList)
                {
                    requestList.Add(new RelationRequest
                    {
                        CompanyId = item.FirstOrDefault().CompanyId,
                        ChannelId = item.Key,
                        IstUserProfile =item.Select(y => new UserProfile
                        {
                            mktuserid = y.UserId,
                            profileid = y.ProfileId
                        }).ToList(),
                    });
                }
                 foreach (var item in requestList)
                {
                    string json = JsonConvert.SerializeObject(new
                    {
                        CompanyID = item.CompanyId,
                        ChannelID = item.ChannelId,
                        IstUserProfile = item.IstUserProfile
                    });
                    response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<string>>(url, json);
                }
            }
            return response;
        }
        #endregion

        #region Save Asignar Timbers Emisor
        public ResponseModel<string> SaveTimberEmisor(TimberRequest req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer ? AzureUrlProd.SaveTimberEmisor : AzureUrl.SaveTimberEmisor;
            string json = JsonConvert.SerializeObject(new
            {
                CompanyID = req.CompanyId,
                ProfileID = req.ProfileId,
                OptionID = req.OptionId
            });
            response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<string>>(url, json);
            return response;
        }
        #endregion

        #region Save Config Auto Setting
        public ResponseModel<string> SaveConfigSetting(ConfiguracionSetting req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = configuracionDAO.SaveConfigSetting(req);
            response.Description = "Ajuste de configuración no guardado.";
            response.Title = "Fallida";
            if (response.Content.Any())
            {
                response.Success = true;
                response.Description = "Ajuste de configuración guardado.";
                response.Title = "Éxito";

            }
            return response;
        }
        #endregion
        #endregion

        #region Usurio

        #region  Get Main Table Users
        public ResponseModel<List<CompanyUser>> GetMainTableUser(int companyId, int page)
        {
            ResponseModel<List<CompanyUser>> response = new ResponseModel<List<CompanyUser>>();
            response.Content = configuracionDAO.GetMainTableUser(companyId, page);
            response.Description = "Usurio no encontrados.";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Lista de usurio.";
                response.Success = true;
            }
            return response;
        }
        #endregion

        #region Get User Data By Id
        public ResponseModel<List<UserData>> GetUserDataById(int companyId, int userId)
        {
            ResponseModel<List<UserData>> response = new ResponseModel<List<UserData>>();
            response.Content = configuracionDAO.GetUserDataById(companyId, userId);
            response.Description = "Usurio no encontrados.";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "De usurio.";
                response.Success = true;
            }
            return response;
        }
        #endregion

        #region Update User Info
        public ResponseModel<string> UpdateUserInfo(UserUpdateDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = configuracionDAO.UpdateUserInfo(req);
            response.Description = "Datos de usuario no actualizados.";
            response.Title = "Fallida";
            if (response.Content.Any())
            {
                response.Success = true;
                response.Description = "Datos de usuario actualizados.";
                response.Title = "Actualizada";
            }
            return response;
        }
        #endregion

        #region Update User Password
        public ResponseModel<string> UpdateUserPassword(UpdatePasswordDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = configuracionDAO.UpdateUserPassword(req);
            response.Description = "Contraseña de usuario no actualizada.";
            response.Title = "Fallida";
            if (response.Content.Any())
            {
                response.Success = true;
                response.Description = "Contraseña de usuario actualizada.";
                response.Title = "Actualizada";
            }
            return response;
        }
        #endregion

        #region Get Permission Tabs
        public ResponseModel<List<PermissionTabVM>> GetPermissionTabs(int moduleId, int userId)
        {
            ResponseModel<List<PermissionTabVM>> response = new ResponseModel<List<PermissionTabVM>>();
            List<PermissionTab> tabs = configuracionDAO.GetPermissionTabs(GetEnumValue<PermissionModule>(moduleId).ToString(), userId);
            response.Description = "Pestañas de permisos de usuario no encontradas.";
            if (tabs != null && tabs.Count > 0)
            {
                response.Content = tabs.GroupBy(x => x.SubModule).Select(y => new PermissionTabVM
                {
                    SubModule = y.Key,
                    Tabs = tabs.Where(z => z.SubModule.Equals(y.Key)).ToList(),
                }).ToList();
                response.Description = "Pestañas de permisos de usuario.";
                response.Success = true;
            }
            return response;
        }

        public static T GetEnumValue<T>(int key) 
        {
                return (T)Enum.ToObject(typeof(T), key);   
        }
        #endregion

        #region Update User Permissions
        public ResponseModel<string> UpdateUserPermission(List<PermissionDTO> req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            if (req.Count > 0)
            {
                foreach (var item in req)
                {
                    response.Content = configuracionDAO.UpdateUserPermission(item);
                    response.Description = "Permisos de usuario no actualizado.";
                    response.Title = "Fallida";
                    if (response.Content.Any())
                    {
                        response.Description = "Permisos de usuario actualizado.";
                        response.Title = "Actualizada";
                        response.Success = true;
                    }
                }
            }
            return response;
        }
        #endregion
        #endregion


        #region Notification

        #region Get Notification Permission List
        public ResponseModel<List<NotifyPermissionVM>> GetNotifyPermissionList(int companyId, int channelId)
        {
            ResponseModel<List<NotifyPermissionVM>> response = new ResponseModel<List<NotifyPermissionVM>>();
            response.Content = new();
            string url = LiveServer ? AzureUrlProd.NotifyPermission : AzureUrl.NotifyPermission;
            string json = JsonConvert.SerializeObject(new
            {
                CompanyID = companyId,
                Channel = channelId
            });
            return AzureResponseHandler.GetAzureResponseObject<ResponseModel<List<NotifyPermissionVM>>>(url, json);
        }

        #endregion

        #region Update Notification
        public ResponseModel<string> UpdateNotification(List<NotifyPermissionVM> req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Description = "Permisos no actualizados correctamente.";
            response.Title = "No lista";
            if (req != null && req.Count > 0)
            {
                foreach (var item in req)
                {
                    UsersVM usersVM = new()
                    {
                        AccountID = item.AccountId,
                        Permissions = item.Permissions.Select(x => new UserPermissionDTO { ID = x.Id, ActiveWP = x.ActiveWP }).ToList()
                        
                    };
                    string url = LiveServer ? AzureUrlProd.UpdatePermission : AzureUrl.UpdatePermission;
                    string json = JsonConvert.SerializeObject(new
                    {
                        CompanyID = req[0].CompanyId,
                        Channel = req[0].ChannelId,
                        Users = usersVM
                    });
                    AzureResponse azureresponse = AzureResponseHandler.GetAzureResponseObject<AzureResponse>(url, json);
                    if (azureresponse != null) 
                    {
                        response.Success = true;
                        response.Title = azureresponse.Title;
                        response.Description = azureresponse.Description;
                    }
                }
            }
            return response;
        }
        #endregion
        #endregion
    }
}
