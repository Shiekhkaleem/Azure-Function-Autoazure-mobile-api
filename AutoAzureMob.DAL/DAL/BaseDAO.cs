
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.DAL.DAL
{
    public class BaseDAO
    {
        #region Creation of data members
        private readonly IConfiguration config;
        public static string connectionString = string.Empty;
        public ExecuteContext ExecutionContext { get; private set; }
        

        //To get queries from resourse file
        protected ResourceManager queryResource = null;

        public BaseDAO(ExecuteContext exeContext, IConfiguration _config)
        {
            config = _config;
            bool LiveServer = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")) || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
            connectionString =LiveServer? config.GetConnectionString("AutoAzure-PROD") : config.GetConnectionString("AutoAzure-DEV");
            ValidateContext(exeContext);

            if (ExecutionContext == null)
            {
                ExecutionContext = exeContext;
            }
            
        }

        //To Validate ExecuteContext that is contains connection or not
        private bool ValidateContext(ExecuteContext exeContext)
        {
            if (exeContext == null || exeContext.Connection == null)
            {
                throw new ArgumentNullException("ExecuteContext can't be null");
            }
            return true;
        }
        #endregion

        #region SqlConnection Methods
        private static void CreateConnection(ExecuteContext ExecutionContext)
        {
            if (ExecutionContext != null && ExecutionContext.Connection == null)
            {
                ExecutionContext.Connection = new SqlConnection(connectionString);
            }
        }
        private static void OpenConnection(ExecuteContext ExecutionContext)
        {
            try
            {
                if (ExecutionContext != null && ExecutionContext.Connection == null)
                {
                    CreateConnection(ExecutionContext);
                }
                if (ExecutionContext.Connection.State == ConnectionState.Closed)
                {
                    ExecutionContext.Connection.Open();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static void CloseConnection(ExecuteContext ExecutionContext)
        {
            try
            {
                if (ExecutionContext != null && ExecutionContext.Connection != null)
                {
                    if (ExecutionContext.Connection.State != ConnectionState.Closed)
                    {
                        ExecutionContext.Connection.Close();
                    }
                    ExecutionContext.Connection.Dispose();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region To Execute Queries
        //To Execute queries specially update
        public int ExecuteNonQuery(ExecuteContext exeContext, string queryName, List<SqlParameter> parameters, bool isStoreProcedure)
        {
            int result = -1;
            try
            {
                using (var cnn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(@queryName, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddRange(parameters.ToArray());
                        if (isStoreProcedure)
                        {
                            cmd.CommandTimeout = 120;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Cancel();
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                        }

                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        //To Execute INSERT UPDATE DELETE queries to get Id in return
        public T ExecuteScalar<T>(ExecuteContext exeContext, string queryName, List<SqlParameter> parameters, bool isStoreProcedure)
        {
            T result = default(T);
            try
            {
                //OpenConnection(exeContext);
                SqlCommand cmd = new SqlCommand();
                if (isStoreProcedure)
                {
                    cmd.CommandText = queryName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                }
                else
                {
                    cmd.CommandText = queryResource.GetString(queryName);
                }
                cmd.Connection = ExecutionContext.Connection;
                cmd.Parameters.AddRange(parameters.ToArray());
                using (cmd)
                {
                    if (ExecutionContext != null && ExecutionContext.Transaction != null)
                    {
                        cmd.Transaction = ExecutionContext.Transaction;
                        result = (T)cmd.ExecuteScalar();
                    }
                    else
                    {
                        OpenConnection(exeContext);
                        using (exeContext.Connection)
                        {
                            result = (T)cmd.ExecuteScalar();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        //To Execute select queries and return a datatable
        public DataTable ExecuteAdapter(string queryName, List<SqlParameter> parameters)
        {
            DataTable resultTable = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(queryResource.GetString(queryName));
                cmd.Connection = ExecutionContext.Connection;
                cmd.Parameters.AddRange(parameters.ToArray());
                using (cmd)
                {
                    if (ExecutionContext != null && ExecutionContext.Transaction != null)
                    {
                        cmd.Transaction = ExecutionContext.Transaction;
                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            ad.Fill(resultTable);
                        }
                    }
                    else
                    {
                        using (ExecutionContext.Connection)
                        {
                            using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                            {
                                ad.Fill(resultTable);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultTable;
        }
        //To Execute select queries and return a datatable
        public DataSet ExecuteAdapter(string queryName, List<SqlParameter> parameters, bool isStoreProcedure)
        {
            DataSet resultSet = new DataSet();
            try
            {
                using (var cnn = new SqlConnection(connectionString))
                {
                    using (var cmd = new SqlCommand(@queryName, cnn))
                    {
                        cnn.Open();
                        cmd.Parameters.AddRange(parameters.ToArray());
                        if (isStoreProcedure)
                        {
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            cmd.CommandType = CommandType.Text;
                        }

                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            ad.Fill(resultSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultSet;
        }
        private string GetQueryValues(List<SqlParameter> sqlParam)
        {
            try
            {
                StringBuilder exString = new StringBuilder("Query values are : ");
                if (sqlParam != null && sqlParam.Count > 0)
                {
                    foreach (SqlParameter item in sqlParam)
                    {
                        exString.Append(item.ParameterName + " : " + item.Value);
                        exString.AppendLine();
                    }
                }
                return exString.ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion
        #region Bulk Insert Into Table
        public void ExecuteBulkInert(DataTable data, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    foreach (DataColumn column in data.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }
                    bulkCopy.WriteToServer(data);
                }
            }
        }
        #endregion
        //All Methods relating to SqlTransactions
        #region Transaction Methods
        public static ExecuteContext CreateExecutionContext(bool createTransaction)
        {
            ExecuteContext result = new ExecuteContext();
            CreateConnection(result);
            if (createTransaction)
            {
                BeginTransaction(result);
            }
            return result;
        }
        public static SqlTransaction BeginTransaction(ExecuteContext ExecutionContext)
        {
            try
            {
                if (ExecutionContext.Connection != null && ExecutionContext.Connection.State == ConnectionState.Closed
                    && ExecutionContext.Transaction == null)
                {
                    ExecutionContext.Connection.Open();
                    ExecutionContext.Transaction = ExecutionContext.Connection.BeginTransaction();
                    return ExecutionContext.Transaction;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void CommitTransaction(ExecuteContext ExecutionContext)
        {
            try
            {
                if (ExecutionContext != null && ExecutionContext.Transaction != null
                    && ExecutionContext.Connection != null)
                {
                   
                    ExecutionContext.Transaction.Commit();
                    CloseConnection(ExecutionContext);
                    ExecutionContext = null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void RollbackTransaction(ExecuteContext ExecutionContext)
        {
            try
            {
                if (ExecutionContext != null && ExecutionContext.Transaction != null
                    && ExecutionContext.Connection != null)
                {
          
                    ExecutionContext.Transaction.Rollback();
                    CloseConnection(ExecutionContext);
                    ExecutionContext = null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
