using AutoAzureMob.Models.Models.Balance;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAzureMob.Models.Models.Facturacion;
using AutoAzureMob.Models.Models.Sale;
using AutoAzureMob.Models.DTO.FacturacionDTO;
using AutoAzureMob.Models.Models.User;

namespace AutoAzureMob.DAL.DAL
{
    public class ConfiguracionDAO : BaseDAO
    {
        private readonly IConfiguration _config;
        public ConfiguracionDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }
        #region Facturacion

        #region Get Profiles 
        public List<Profile> GetProfileList(int companyId,int page)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
               new SqlParameter("@Page",page)
            };
            string queryName = "MOB_INV_getprofilelist";
            List<Profile> response = FetchProfileList(queryName, param);
            return response;
        }

        private List<Profile> FetchProfileList(string queryName, List<SqlParameter> param)
        {
            List<Profile> list = new List<Profile>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            Profile profile = new Profile();
                            profile.ProfileId = row["ProfileID"].ToString() ?? string.Empty;
                            profile.TaxId = row["TaxID"].ToString() ?? string.Empty;
                            profile.TaxName = row["TaxName"].ToString() ?? string.Empty;
                            profile.Address = row["Address"].ToString() ?? string.Empty;
                            profile.TotalRows = Convert.ToInt32(row["TotalRows"] ?? default);
                            list.Add(profile);
                        }

                    }
                }

            }
            return list;
        }
        #endregion

        #region Get User Relacion List
        public List<Relacion> GetUserRelacionList(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
            };
            string queryName = "MOB_INV_getuserprofile_relation";
            List<Relacion> response = FetchUserRelacionList(queryName, param);
            return response;
        }

        private List<Relacion> FetchUserRelacionList(string queryName, List<SqlParameter> param)
        {
            List<Relacion> list = new List<Relacion>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            Relacion relacion = new Relacion();
                            relacion.ChannelId = !string.IsNullOrEmpty(row["channelid"].ToString())?Convert.ToInt32(row["channelid"]): default;
                            relacion.MKTUserId = row["mktuserid"].ToString() ?? string.Empty;
                            relacion.UserName = row["username"].ToString() ?? string.Empty;
                            relacion.ProfileId = row["profileid"].ToString() ?? string.Empty;
                            list.Add(relacion);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Relacion Field List
        public List<RelacionField> GetRelacionFieldList(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
            };
            string queryName = "MOB_INV_getprofileids";
            List<RelacionField> response = FetchRelacionFieldList(queryName, param);
            return response;
        }

        private List<RelacionField> FetchRelacionFieldList(string queryName, List<SqlParameter> param)
        {
            List<RelacionField> list = new List<RelacionField>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            RelacionField relacion = new RelacionField();
                            relacion.Id = row["id"].ToString() ?? string.Empty;
                            relacion.Name = row["name"].ToString() ?? string.Empty; 
                            list.Add(relacion);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Timbers Perfile  List
        public List<TimberDropDown> GetTimberProfileList(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
            };
            string queryName = "MOB_INV_getprofileids";
            List<TimberDropDown> response = FetchTimberProfileList(queryName, param);
            return response;
        }

        private List<TimberDropDown> FetchTimberProfileList(string queryName, List<SqlParameter> param)
        {
            List<TimberDropDown> list = new List<TimberDropDown>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            TimberDropDown profile = new TimberDropDown();
                            profile.Id = row["id"].ToString() ?? string.Empty;
                            profile.Name = row["name"].ToString() ?? string.Empty;
                            list.Add(profile);
                        }

                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Timbers List
        public List<DropDown> GetTimberList()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
            };
            string queryName = "MOB_INV_getTimbreslist";
            List<DropDown> response = FetchTimberList(queryName, param);
            return response;
        }

        private List<DropDown> FetchTimberList(string queryName, List<SqlParameter> param)
        {
            List<DropDown> list = new List<DropDown>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            DropDown profile = new DropDown();
                            profile.Id = !string.IsNullOrEmpty(row["id"].ToString()) ? Convert.ToInt32(row["id"]) : default;
                            profile.Name = row["name"].ToString() ?? string.Empty;
                            list.Add(profile);
                        }
                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Configuracion FormadePago List
        public List<DropDown> GetConfigFormadePagoList()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
            };
            string queryName = "MOB_CAT_getformadepago";
            List<DropDown> response = FetchConfigFormadePagoList(queryName, param);
            return response;
        }

        private List<DropDown> FetchConfigFormadePagoList(string queryName, List<SqlParameter> param)
        {
            List<DropDown> list = new List<DropDown>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            DropDown profile = new DropDown();
                            profile.Id = !string.IsNullOrEmpty(row["ID"].ToString()) ? Convert.ToInt32(row["id"]) : default;
                            profile.Name = row["Name"].ToString() ?? string.Empty;
                            list.Add(profile);
                        }
                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Configuracion Setting
        public ConfiguracionSetting GetConfiguracionSetting(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
            };
            string queryName = "MOB_INV_CONFIG_loadautoinv_v2";
            ConfiguracionSetting response = FetchConfiguracionSetting(queryName, param);
            return response;
        }

        private ConfiguracionSetting FetchConfiguracionSetting(string queryName, List<SqlParameter> param)
        {
            ConfiguracionSetting setting = new ConfiguracionSetting();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            setting.IncludeShippingCost = Convert.ToBoolean(row["addshipping"]);
                            setting.IncludeReference = Convert.ToBoolean(row["showSaleRef"]);
                            setting.GenerateCreditNote = Convert.ToBoolean(row["createnote"]);
                            setting.GlobalSkuInvoice = Convert.ToBoolean(row["descProduct"]);
                            setting.ValidDays = !string.IsNullOrEmpty(row["expDays"].ToString()) ? Convert.ToInt32(row["expDays"]) : default;
                            setting.AllowFP = !string.IsNullOrEmpty(row["allowfp"].ToString()) ? Convert.ToInt32(row["allowfp"]) : default;
                            setting.FormaPago = row["FormaPago"].ToString() ?? "";

                        }

                    }
                }

            }
            return setting;
        }
        #endregion

        #region Save Config Auto Setting
        public string SaveConfigSetting(ConfiguracionSetting req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",req.CompanyId),
               new SqlParameter("@AllowFP",req.AllowFP),
               new SqlParameter("@AddShiping",req.IncludeShippingCost),
               new SqlParameter("@CreateNote",req.GenerateCreditNote),
               new SqlParameter("@FormaPago",req.FormaPago),
               new SqlParameter("@expDays",req.ValidDays),
               new SqlParameter("@descProduct",req.GlobalSkuInvoice),
               new SqlParameter("@showSaleRef",req.IncludeReference),
            };
            string queryName = "MOB_INV_CONFIG_saveautoinv_v2";
            string response = FetchCompanyId(queryName, param);
            return response;
        }
        private string FetchCompanyId(string queryName, List<SqlParameter> param)
        {
            string response = string.Empty;
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            response = row["CompanyID"].ToString() ?? "";
                        }

                    }
                }

            }
            return response;
        }
        #endregion
        #endregion


        #region Usurio

        #region Get Main Table Users
        public List<CompanyUser> GetMainTableUser(int companyId, int page)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@companyid",companyId),
               new SqlParameter("@Page",page),
            };
            string queryName = "MOB_USER_getmaintable";
            List<CompanyUser> response = FetchMainTableUser(queryName, param);
            return response;
        }

        private List<CompanyUser> FetchMainTableUser(string queryName, List<SqlParameter> param)
        {
            List<CompanyUser> list = new List<CompanyUser>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            CompanyUser user = new CompanyUser();
                            user.UserId = !string.IsNullOrEmpty(row["userid"].ToString()) ? Convert.ToInt32(row["userid"]) : default;
                            user.Numero = row["Numero"].ToString() ?? "";
                            user.Name = row["name"].ToString() ?? "";
                            user.UserName = row["username"].ToString() ?? "";
                            user.Email = row["email"].ToString() ?? "";
                            user.Store = row["store"].ToString() ?? "";
                            user.TotalRows = Convert.ToInt32(row["TotalRows"] ?? default);
                            list.Add(user);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get User Data By Id
        public List<UserData> GetUserDataById(int companyId, int userId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@CompanyID",companyId),
                new SqlParameter("@EditUserID",userId)
            };
            string queryName = "MOB_USER_getuserdata";
            List<UserData> response = FetchUserDataById(queryName, param);
            return response;
        }

        private List<UserData> FetchUserDataById(string queryName, List<SqlParameter> param)
        {
            List<UserData> list = new List<UserData>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            UserData data = new UserData();
                            data.UserId = !string.IsNullOrEmpty(row["edituserid"].ToString()) ? Convert.ToInt32(row["edituserid"]) : default;
                            data.Number = row["number"].ToString() ?? "";
                            data.Name = row["name"].ToString() ?? "";
                            data.UserName = row["username"].ToString() ?? "";
                            data.Email = row["email"].ToString() ?? "";
                            data.StoreId = !string.IsNullOrEmpty(row["storeid"].ToString()) ? Convert.ToInt32(row["storeid"]) : default;
                            data.PriceId = !string.IsNullOrEmpty(row["priceid"].ToString()) ? Convert.ToInt32(row["priceid"]) : default;
                            list.Add(data);
                        }
                    }
                }

            }
            return list;
        }
        #endregion

        #region Update User Info
        public string UpdateUserInfo(UserUpdateDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@CompanyID",req.CompanyId),
                new SqlParameter("@EditUserID",req.EditUserId),
                new SqlParameter("@Name",req.Name),
                new SqlParameter("@UserName",req.UserName),
                new SqlParameter("@Email",req.Email)
            };
            string queryName = "MOB_USER_saveuser_general";
            string response = FetchUpdateUserInfoResponse(queryName, param);
            return response;
        }
        private string FetchUpdateUserInfoResponse(string queryName, List<SqlParameter> param)
        {
            string response = string.Empty;
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            response = row["edituserid"].ToString() ?? "";
                        }

                    }
                }

            }
            return response;
        }
        #endregion

        #region Update User Password
        public string UpdateUserPassword(UpdatePasswordDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@CompanyID",req.CompanyId),
                new SqlParameter("@UserID",req.UserId),
                new SqlParameter("@Password",req.OldPassword),
                new SqlParameter("@NewPassword",req.NewPassword),
            };
            string queryName = "MOB_USER_saveuser_password";
            string response = FetchUpdateUserPasswordResponse(queryName, param);
            return response;
        }
        private string FetchUpdateUserPasswordResponse(string queryName, List<SqlParameter> param)
        {
            string response = string.Empty;
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            response = row["userid"].ToString() ?? "";
                        }

                    }
                }

            }
            return response;
        }
        #endregion

        #region Get Permission Tabs
        public List<PermissionTab> GetPermissionTabs(string tabName, int userId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@tabname",tabName),
               new SqlParameter("@edituserid",userId),
            };
            string queryName = "MOB_USER_gettaboptions";
            List<PermissionTab> response = FetchPermissionTabs(queryName, param);
            return response;
        }

        private List<PermissionTab> FetchPermissionTabs(string queryName, List<SqlParameter> param)
        {
            List<PermissionTab> list = new List<PermissionTab>();
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            PermissionTab tab = new PermissionTab();
                            tab.PermissionId = !string.IsNullOrEmpty(row["PermissionID"].ToString()) ? Convert.ToInt32(row["PermissionID"]) : default;
                            tab.SubModule = row["SubModule"].ToString()?? string.Empty;
                            tab.PermissionName = row["PermissionName"].ToString() ?? string.Empty;
                            tab.PermissionDescription = row["PermissionDescription"].ToString() ?? string.Empty;
                            tab.Active = Convert.ToBoolean(row["Active"]);
                            list.Add(tab);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Update User Permissions
        public string UpdateUserPermission(PermissionDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
                new SqlParameter("@CompanyID",req.CompanyId),
                new SqlParameter("@EditUserID",req.EditUserId),
                new SqlParameter("@PermissionID",req.PermissionId),
                new SqlParameter("@Active",req.Active),
            };
            string queryName = "MOB_USER_saveuser_permissionid";
            string response = FetchUpdateUserPermission(queryName, param);
            return response;
        }
        private string FetchUpdateUserPermission(string queryName, List<SqlParameter> param)
        {
            string response = string.Empty;
            DataSet resultSet = null;
            if (!String.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, param, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            response = row["userid"].ToString() ?? "";
                        }

                    }
                }

            }
            return response;
        }
        #endregion
        #endregion

    }
}
