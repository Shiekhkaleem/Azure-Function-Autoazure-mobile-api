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
    public class CompanyDAO : BaseDAO
    {
        private readonly IConfiguration _config;
        public CompanyDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config) 
        {
            _config = config;
        }
        #region GetAllCompanyRoles
        public List<CompanyRole> GetAllCompanyRoles()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            { };
            string queryName = "[dbo].[CAT_getcompanyrole]";
            List<CompanyRole> response = FetchAllCompanyRoles(queryName,param);
            return response?? new List<CompanyRole>();
        }
        private List<CompanyRole> FetchAllCompanyRoles(string queryName, List<SqlParameter> param)
        {
            List<CompanyRole> list = new List<CompanyRole>();
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
                            CompanyRole role = new CompanyRole();
                            role.RoleId = !string.IsNullOrEmpty(row["id"].ToString())? Convert.ToInt32(row["id"]) : 0;
                            role.RoleName = row["name"].ToString() ?? "";
                            list.Add(role);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

    }
}
