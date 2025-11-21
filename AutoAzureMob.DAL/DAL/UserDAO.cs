using AutoAzureMob.Models.DTO.UserDTO;
using AutoAzureMob.Models.Models.Company;
using AutoAzureMob.Models.Models.User;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class UserDAO : BaseDAO
    {
        private readonly IConfiguration config;
        public UserDAO(ExecuteContext executionContext, IConfiguration _config) : base(executionContext, _config)
        {
            config = _config;
        }

        #region Validate User
        public UserInfo validateCredentials(LoginRequest login)
        {
            List<SqlParameter> param = new List<SqlParameter>()
             {
               new SqlParameter("@companycode",login.CompanyCode),
               new SqlParameter("@usuario",login.UserName),
               new SqlParameter("@password",login.Password)
             };
            string queryName = "GRL_MOB_validatecredentials";
            UserInfo response = FetchValidateCredentials(queryName, param);
            return response;
        }
        //Fetch Validate response
        private UserInfo FetchValidateCredentials(string queryName, List<SqlParameter> param)
        {
            UserInfo user =new UserInfo();
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
                            user.Token = row["token"].ToString() ?? "";
                            user.UserId = !string.IsNullOrEmpty(row["userid"].ToString()) ? Convert.ToInt32(row["userid"]) : 0;
                            user.CompanyId = !string.IsNullOrEmpty(row["companyid"].ToString()) ? Convert.ToInt32(row["companyid"]) : 0;
                            user.CompanyName = row["companyname"].ToString() ?? "";
                            user.CompanyBrand = row["companybrand"].ToString()?? "" ;
                            user.FirstStart = !string.IsNullOrEmpty(row["firststart"].ToString())? Convert.ToBoolean(row["firststart"]) : false;
                            user.Expired = !string.IsNullOrEmpty(row["expired"].ToString()) ? Convert.ToBoolean(row["expired"]): false;
                            user.LicenseStatus = row["licensestatus"].ToString() ?? "";
                            user.DaysLeft = !string.IsNullOrEmpty(row["daysleft"].ToString()) ? Convert.ToInt32(row["daysleft"]) : 0;
                            user.StatusName = row["statusname"].ToString()?? string.Empty;
                            user.IsAdmin = !string.IsNullOrEmpty(row["IsAdmin"].ToString()) ? Convert.ToBoolean(row["IsAdmin"]) : false;
                            user.ForcePass = !string.IsNullOrEmpty(row["forcepass"].ToString()) ? Convert.ToBoolean(row["forcepass"]) : false;
                            user.Email = row["email"].ToString() ?? "";
                            user.Modality = !string.IsNullOrEmpty(row["modality"].ToString()) ? Convert.ToBoolean(row["modality"]) : false;
                            user.Comp_Mod = !string.IsNullOrEmpty(row["comp_mod"].ToString()) ? Convert.ToBoolean(row["comp_mod"]) : false;
                            user.Ml_UnlinkedAccount = row["ml_UnlinkedAccount"].ToString().Equals("1") ? true : false;
                            user.DemoDaysLeft = !string.IsNullOrEmpty(row["DemoDaysLeft"].ToString()) ? Convert.ToInt32(row["DemoDaysLeft"]) : 0;
                            user.InfoUpdateCFDI40 = !string.IsNullOrEmpty(row["InfoUpdateCFDI40"].ToString()) ? Convert.ToBoolean(row["InfoUpdateCFDI40"]) : false;
                        }

                    }
                }

            }
            return user;
        }
        #endregion
        #region Verify Email
        public string VerifyEmailAlreadyExist(string Email)
        {
            List<SqlParameter> param = new List<SqlParameter>
                                        {
                #region Setting Login User values to DB param
                new SqlParameter("@email", Email)
                #endregion
            };
            string queryName = "GRL_ValidateEmail";
            string userId = FetchGenericResponse(param, queryName);
            return userId;
        }
        private string FetchGenericResponse(List<SqlParameter> sqlParam, string queryName)
        {
            string response = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable loginTable = resultSet.Tables[0];
                    if (loginTable.Rows.Count > 0)
                    {
                        response = string.Empty;
                        foreach (DataRow row in loginTable.Rows)
                        {
                            #region adding 

                            if (loginTable.Columns.Contains("result"))
                            {
                                response = row["result"].ToString();
                            }
                            else if (loginTable.Columns.Contains("userid"))
                            {
                                response = row["userid"].ToString();
                            }
                            #endregion
                        }
                    }
                }
            }
            return response;
        }
        #endregion
        #region User Registration
        public UserRegistration RegisterSystemUser(UserRegistration createUser)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@Name", createUser.FirstLastName),
                new SqlParameter("@CompanyName", createUser.CompanyName),
                new SqlParameter("@Phone", createUser.Phone),
                new SqlParameter("@Email", createUser.Email),
                new SqlParameter("@UserName", createUser.UserName),
                new SqlParameter("@Password", createUser.Password),
                new SqlParameter("@Source", createUser.Source),
                #endregion
            };
            string queryName = "GRL_UserRegister";
            UserRegistration regUser = FetchRegisterUserDetails(param, queryName);
            return regUser;
        }
        private UserRegistration FetchRegisterUserDetails(List<SqlParameter> sqlParam, string queryName)
        {
            UserRegistration user = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable loginTable = resultSet.Tables[0];
                    if (loginTable.Rows.Count > 0)
                    {
                        user = new UserRegistration();
                        foreach (DataRow row in loginTable.Rows)
                        {
                            #region adding 

                            user.CompanyCode = row["companycode"].ToString();
                            user.UserId = row["userid"].ToString();
                            user.Email = row["email"].ToString();
                            user.UserName = row["username"].ToString();
                            user.Password = row["password"].ToString();

                            #endregion
                        }
                    }
                }
            }
            return user;
        }
        #endregion
        #region Admin Commpany Registration
        public RegistResponse AdminCompanyRegistration(RegistRequest company)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@MainContact", company.DisplayName),
                new SqlParameter("@CompanyName", company.CompanyName),
                new SqlParameter("@MainPhone", company.MainPhone),
                new SqlParameter("@MainEmail", company.MainEmail),
                new SqlParameter("@Password", company.Password),
                new SqlParameter("@RoleID", company.RoleId),
                new SqlParameter("@modalityProfile", company.ModalityProfile),
                #endregion
            };
            string queryName = "MOB_SP_createcompanyuser";
            RegistResponse regUser = FetchCompanyRegistration(param, queryName);
            return regUser;
        }
        private RegistResponse FetchCompanyRegistration(List<SqlParameter> sqlParam, string queryName)
        {
            RegistResponse response = new RegistResponse();
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable loginTable = resultSet.Tables[0];
                    if (loginTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in loginTable.Rows)
                        {
                            response.CompanyCode = row["CompanyCode"].ToString();
                            response.UserName = row["UserName"].ToString();
                            response.Password = row["Password"].ToString();
                            response.RoleName = row["RoleName"].ToString();
                            response.CompanyBrand = row["CompanyBrand"].ToString();
                            response.Result = row["result"].ToString();
                            response.ResultText= row["resultext"].ToString();
                            response.UniGuide= row["uniguid"].ToString();
                        }
                    }
                }
            }
            return response;
        }
        #endregion
        #region Activate User Account
        public string ActivateUserAccount(string userId)
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                #region Setting Login User values to DB param
                new SqlParameter("@UserID", userId)
                #endregion
            };
            string queryName = "GRL_ActivateUser";
            string regUser = FetchGenericResponse(param, queryName);
            return regUser;
        }
        #endregion
        #region Verify User Token
        public UserInfo VerifyUserToken(UserInfo userLogin)
        {
            List<SqlParameter> param = new List<SqlParameter>
                                        {
                #region Setting Login User values to DB param
                new SqlParameter("@token", userLogin.Token),
                new SqlParameter("@userid", userLogin.UserId),
                #endregion
            };
            string queryName = "GRL_validateusertoken";
            UserInfo userTokenVerified = FetchLoginDetails(param, queryName);
            return userTokenVerified;
        }
        private UserInfo FetchLoginDetails(List<SqlParameter> sqlParam, string queryName)
        {
            UserInfo userLogin = null;
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable loginTable = resultSet.Tables[0];
                    if (loginTable.Rows.Count > 0)
                    {
                        userLogin = new UserInfo();
                        foreach (DataRow row in loginTable.Rows)
                        {
                            #region adding 

                            userLogin.CompanyId = Convert.ToInt32(row["companyid"]);
                            userLogin.UserId =Convert.ToInt32(row["userid"]);

                            if (row.Table.Columns.Contains("token"))
                            {
                                userLogin.Token = row["token"].ToString();
                            }
                            if (row.Table.Columns.Contains("displayname"))
                            {
                                userLogin.ContactName = row["displayname"].ToString();
                            }
                            if (row.Table.Columns.Contains("companyname"))
                            {
                                userLogin.CompanyName = row["companyname"].ToString();
                            }
                            if (row.Table.Columns.Contains("companybrand"))
                            {
                                userLogin.CompanyBrand = row["companybrand"].ToString();
                            }
                            if (row.Table.Columns.Contains("firststart"))
                            {
                                userLogin.FirstStart = row["firststart"].ToString() == "" ? false : Convert.ToBoolean(row["firststart"].ToString());
                            }
                            if (row.Table.Columns.Contains("uniqueguid"))
                            {
                                userLogin.UniqueGuid = row["uniqueguid"].ToString();
                            }

                            if (row.Table.Columns.Contains("expired"))
                            {
                                userLogin.Expired = Convert.ToBoolean(row["expired"].ToString());
                            }
                            if (row.Table.Columns.Contains("licensestatus"))
                            {
                                userLogin.LicenseStatus = row["licensestatus"].ToString();
                            }

                            if (row.Table.Columns.Contains("daysleft"))
                            {
                                userLogin.DaysLeft =Convert.ToInt32(row["daysleft"]);
                            }

                            if (row.Table.Columns.Contains("statusname"))
                            {
                                userLogin.StatusName = row["statusname"].ToString();
                            }

                            if (row.Table.Columns.Contains("IsAdmin"))
                            {
                                userLogin.IsAdmin = Convert.ToBoolean(row["IsAdmin"].ToString());
                            }

                            if (row.Table.Columns.Contains("forcepass"))
                            {
                                userLogin.ForcePass = Convert.ToBoolean(row["forcepass"].ToString());
                            }

                            if (row.Table.Columns.Contains("email"))
                            {
                                userLogin.Email = row["email"].ToString();
                            }

                            if (row.Table.Columns.Contains("modality"))
                            {
                                userLogin.Modality = Convert.ToBoolean(row["modality"].ToString());
                            }

                            if (row.Table.Columns.Contains("comp_mod"))
                            {
                                userLogin.Comp_Mod = Convert.ToBoolean(row["comp_mod"].ToString());
                            }

                            if (row.Table.Columns.Contains("InfoUpdateCFDI40"))
                            {
                                userLogin.InfoUpdateCFDI40 = Convert.ToBoolean(row["InfoUpdateCFDI40"].ToString());
                            }

                            if (row.Table.Columns.Contains("ml_UnlinkedAccount"))
                            {
                                userLogin.Ml_UnlinkedAccount = row["ml_UnlinkedAccount"].ToString().Equals("1") ? true : false;
                            }

                            if (row.Table.Columns.Contains("DemoDaysLeft"))
                            {
                                userLogin.DemoDaysLeft = Convert.ToInt32(row["DemoDaysLeft"].ToString());
                            }

                            #endregion
                        }
                    }
                }
            }
            return userLogin;
        }
        #endregion
        #region Forget Password
        public ForgetPassResponse ForgetPassRequest(ForgetPassDTO forget)
        {
            List<SqlParameter> param = new List<SqlParameter>
                                        {
                #region Setting Login User values to DB param
                new SqlParameter("@Company", forget.CompanyCode),
                new SqlParameter("@Username", forget.UserName),
                #endregion
            };
            string queryName = "MOB_USER_RESET_createlink";
            ForgetPassResponse userTokenVerified = FetchForgetPassResponse(param, queryName);
            return userTokenVerified;
        }
        private ForgetPassResponse FetchForgetPassResponse(List<SqlParameter> sqlParam, string queryName)
        {
            ForgetPassResponse response = new ForgetPassResponse();
            DataSet resultSet = null;
            if (!string.IsNullOrWhiteSpace(queryName))
            {
                resultSet = ExecuteAdapter(queryName, sqlParam, true);
                if (resultSet != null && resultSet.Tables.Count > 0)
                {
                    DataTable Table = resultSet.Tables[0];
                    if (Table.Rows.Count > 0)
                    {
                        foreach (DataRow row in Table.Rows)
                        {
                            #region adding 

                            if (Table.Columns.Contains("result"))
                            {
                                response.Result = row["result"].ToString()?? "";
                            }
                            if (Table.Columns.Contains("token"))
                            {
                                if (!string.IsNullOrEmpty(row["token"].ToString()))
                                {
                                    byte[] bytes = (byte[])row["token"];
                                    response.Token = BitConverter.ToString(bytes).Replace("-", "") ?? "";
                                }
                            }
                            if (Table.Columns.Contains("email"))
                            {
                                response.Email = row["email"].ToString()?? "";
                            }
                            if (Table.Columns.Contains("maincontact"))
                            {
                                response.MainContact = row["maincontact"].ToString() ?? "";
                            }
                            #endregion
                        }
                    }
                }
            }
            return response;
        }
        #endregion
        #region Delete DeviceToken
        public string DeleteUserDeviceToken(int userId, string type)
        {
            List<SqlParameter> param = new List<SqlParameter>()
          {
            new SqlParameter("@UserId",userId),
            new SqlParameter("@Type",type),
          };
            string queryName = "MOB_NOTIFY_DeleteUserDeviceToken";
            string response = ExecuteNonQuery(ExecutionContext, queryName, param, true).ToString();
            return response;
        }
        #endregion
    }
}
