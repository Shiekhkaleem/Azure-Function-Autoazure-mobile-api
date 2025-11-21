using AutoAzureMob.Models.Models.Balance;
using AutoAzureMob.Models.Models.OmniChannel;
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
    public class BalanceDAO   : BaseDAO
    {
        private readonly IConfiguration _config;
        public BalanceDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }

        #region MyRegion
        public List<PaymentOrder> GetPaymentOrderList(int companyId, int page)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@companyid",companyId),
               new SqlParameter("@Page",page),
            };
            string queryName = "MOB_PAY_getpaymentordersV2";
            List<PaymentOrder> response = FetchPaymentOrderList(queryName, param);
            return response;
        }

        private List<PaymentOrder> FetchPaymentOrderList(string queryName, List<SqlParameter> param)
        {
            List<PaymentOrder> list = new List<PaymentOrder>();
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
                            PaymentOrder order = new PaymentOrder();
                            order.Id = !string.IsNullOrEmpty(row["id"].ToString()) ? Convert.ToInt32(row["id"]) : 0; ;
                            order.Folio = row["folio"].ToString() ?? "";
                            order.InvDate = row["invdate"].ToString() ?? "";
                            order.Description = row["description"].ToString() ?? "";
                            order.Amount =!string.Equals( row["amount"].ToString(),"0") ? Convert.ToDecimal(row["amount"]).ToString("$ #,###.##"): "$0";
                            order.Balance = !string.Equals(row["balance"].ToString(), "0") ? Convert.ToDecimal(row["balance"]).ToString("$ #,###.##") : "$0";
                            order.InvType = row["invtype"].ToString() ?? "";
                            order.StatusId = !string.IsNullOrEmpty(row["statusid"].ToString()) ? Convert.ToInt32(row["statusid"]) : 0;
                            order.Status = row["status"].ToString() ?? "";
                            order.DueDate = row["duedate"].ToString() ?? "";
                            order.InvoiceId = !string.IsNullOrEmpty(row["invoiceid"].ToString()) ? Convert.ToInt32(row["invoiceid"]) : 0;
                            order.NotTransfer = Convert.ToBoolean(row["nottransfer"]);
                            order.OL = Convert.ToInt32(row["OL"] ?? 0);
                            order.TotalRows = Convert.ToInt32(row["TotalRows"] ?? 0);
                            list.Add(order);
                        }

                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Card CurrentBalance
        public CardBalance GetCurrentCardBalance(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@companyid",companyId),
            };
            string queryName = "MOB_PAY_getcardcurrentbalance";
            CardBalance response = FetchCurrentCardBalance(queryName, param);
            return response;
        }

        private CardBalance FetchCurrentCardBalance(string queryName, List<SqlParameter> param)
        {
            CardBalance card = new CardBalance();
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
                            card.CurrentBalance = !string.Equals(row["currentbalance"].ToString(), "0") ? Convert.ToDecimal(row["currentbalance"]).ToString("$ #,###.##") : "$0";
                            card.Message = row["message"].ToString() ?? string.Empty;
                            card.OpenPayment = row["openpayment"].ToString() ?? string.Empty;
                            card.Result = row["result"].ToString() ?? string.Empty;
                        }
                    }
                }

            }
            return card;
        }
        #endregion

        #region Get Export List
        public List<Export> GetExportList(int userId, int page)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@UserID",userId),
               new SqlParameter("@Page",page),
            };
            string queryName = "MOB_EXP_GetRequests";
            List<Export> response = FetchExportList(queryName, param);
            return response;
        }

        private List<Export> FetchExportList(string queryName, List<SqlParameter> param)
        {
            List<Export> list = new List<Export>();
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
                            Export export = new Export();
                            export.MarketPlace = row["Marketplace"].ToString() ?? "";
                            export.Reporte = row["Reporte"].ToString() ?? "";
                            export.Cuenta = row["Cuenta"].ToString() ?? "";
                            export.Filtro = row["Filtro"].ToString() ?? "";
                            export.FechaPeticion = row["FechaPeticion"].ToString() ?? "";
                            export.Estado = row["Estado"].ToString() ?? "";
                            export.Porcentaje = row["Porcentaje"].ToString() ?? "";
                            export.FechaActualizacion = row["FechaActualizacion"].ToString() ?? "";
                            export.OutPut = row["Output"].ToString() ?? "";
                            export.TotalRows = Convert.ToInt32(row["TotalRows"] ?? 0);
                            list.Add(export);
                        }

                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Import List
        public List<Import> GetImportList(int userId, int page)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@UserID",userId),
               new SqlParameter("@Page",page),
            };
            string queryName = "MOB_IMP_GetRequests";
            List<Import> response = FetchImportList(queryName, param);
            return response;
        }

        private List<Import> FetchImportList(string queryName, List<SqlParameter> param)
        {
            List<Import> list = new List<Import>();
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
                            Import import = new Import();
                            import.MarketPlace = row["Marketplace"].ToString() ?? "";
                            import.ID = !string.IsNullOrEmpty(row["ID"].ToString()) ? Convert.ToInt64(row["ID"]) : 0;
                            import.Cuenta = row["Cuenta"].ToString() ?? "";
                            import.Tipo = row["Tipo"].ToString() ?? "";
                            import.URLArchivo = row["URLArchivo"].ToString() ?? "";
                            import.Archivo = row["Archivo"].ToString() ?? "";
                            import.NombreArchivo = row["NombreArchivo"].ToString() ?? "";
                            import.Contenedor = row["Contenedor"].ToString() ?? "";
                            import.FechaPeticion = row["FechaPeticion"].ToString() ?? "";
                            import.EstadoValue = row["Estado"].ToString() ?? "";
                            import.Porcentaje = row["Porcentaje"].ToString() ?? "";
                            import.FechaActualizacion = row["FechaActualizacion"].ToString() ?? "";
                            import.ArchivoError = row["ArchivoError"].ToString() ?? "";
                            import.ChannelId = !string.IsNullOrEmpty(row["ChannelID"].ToString()) ? Convert.ToInt32(row["ChannelID"]) : 0;
                            import.TotalRows = Convert.ToInt32(row["TotalRows"] ?? 0);
                            list.Add(import);
                        }

                    }
                }

            }
            return list;
        }
        #endregion
        #region Insert Bulk Query Demo
        public void InsertUserTestRecords(List<string> req)
        { 
            //Create data table for mapping with database table
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(Int32)));
            data.Columns.Add(new DataColumn("FirstName", typeof(string)));
            data.Columns.Add(new DataColumn("Age", typeof(Int32)));
            data.Columns.Add(new DataColumn("IsActive", typeof(bool)));

            foreach (var item in req)
            {
                DataRow row = data.NewRow();
                row["FirstName"] = "Kaleem";
                row["Age"] = 28;
                row["IsActive"] = false;
                  data.Rows.Add(row);
            }
            string tableName = "Person";
            ExecuteBulkInert(data, tableName);

        }
        #endregion
    }
}
