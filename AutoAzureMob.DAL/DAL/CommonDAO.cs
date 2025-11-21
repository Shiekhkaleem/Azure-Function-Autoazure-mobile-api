using AutoAzureMob.Models.Models.Sale;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class CommonDAO      : BaseDAO
    {
        private readonly IConfiguration _config;
        public CommonDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }
        public string FetchGenericColumn(string queryName, List<SqlParameter> param)
        {
            string result = string.Empty;
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
                            result = row["Result"].ToString() ?? "";
                        }
                    }
                }
            }
            return result;
        }

        public string FetchGenericNoColumnName(string queryName, List<SqlParameter> param)
        {
            string result = string.Empty;
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
                            result = row[0].ToString() ?? "";
                        }
                    }
                }
            }
            return result;
        }
    }
}
