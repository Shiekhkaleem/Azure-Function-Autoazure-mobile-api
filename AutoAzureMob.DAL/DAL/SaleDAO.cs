using AutoAzureMob.Models.DTO.SaleDTO;
using AutoAzureMob.Models.Models.Sale;
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
    public class SaleDAO : BaseDAO
    {
        private readonly IConfiguration config;
        private readonly ExecuteContext _executeContext;
        private readonly CommonDAO commonDAO;
        public SaleDAO(ExecuteContext executionContext, IConfiguration _config) : base(executionContext, _config)
        {
            config = _config;
            _executeContext = executionContext;
            commonDAO = new CommonDAO(executionContext, config);
        }
        #region Get Sale Orders List
        public List<SaleOrders> GetSaleOrders(OrderRequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",req.CompanyId),
               new SqlParameter("@StartDate",req.StartDate),
               new SqlParameter("@EndDate",req.EndDate),
               new SqlParameter("@InvoiceTypeID",req.InvoiceTypeId),
               new SqlParameter("@DeliveryTypeID",req.DeliveryTypeId),
               new SqlParameter("@CustomerID",req.CustomerId),
               new SqlParameter("@References",req.References),
               new SqlParameter("@Channels",req.Channels),
               new SqlParameter("@Status",req.Status),
               new SqlParameter("@Page",req.Page),
               new SqlParameter("@Limit",req.Limit),
            };
            string queryName = "MOB_SALE_GetOrders";
            List<SaleOrders> response = FetchSaleOrders(queryName,param);
            return response;    
        }
        private List<SaleOrders> FetchSaleOrders(string queryName, List<SqlParameter> param)
        {
            List<SaleOrders> list = new List<SaleOrders>();
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
                            SaleOrders orders = new SaleOrders();
                            orders.SaleId =Convert.ToInt64( row["SaleID"] ?? 0);
                            orders.Folio = row["Folio"].ToString() ?? "";
                            orders.SaleDate = row["SaleDate"].ToString()?? "";
                            orders.CustomerId = !string.IsNullOrEmpty(row["CustomerID"].ToString()) ? Convert.ToInt32(row["CustomerID"]) : default;
                            orders.Customer = row["Customer"].ToString()?? "";
                            orders.Email = row["Email"].ToString()?? "";
                            orders.ChannelId = Convert.ToInt32(row["ChannelID"] ?? 0);
                            orders.ChannelName = row["Channel"].ToString() ?? "";
                            orders.DeliveryMethod = row["DeliveryMethod"].ToString() ?? "";
                            orders.Store = row["Store"].ToString() ?? "";
                            orders.WareHouse = row["WareHouse"].ToString() ?? "";
                            orders.Reference = row["Reference"].ToString() ?? "";
                            orders.Total= row["Total"].ToString() ?? "";
                            orders.InvoiceId = Convert.ToInt32(row["InvoiceId"] ?? 0);
                            orders.Invoice = row["Invoice"].ToString() ?? "";
                            orders.StatusId = Convert.ToInt32(row["StatusId"] ?? 0);
                            orders.Status = row["Status"].ToString() ?? "";
                            orders.StatusDate = row["StatusDate"].ToString()?? "";
                            orders.TotalRows = Convert.ToInt32(row["TotalRows"] ?? 0);
                            list.Add(orders);   
                        }

                    }
                }

            }
            return list;
        }
        #endregion

        #region Get Invoice Types
        public List<DropDown> GetInvoiceTypes()
        {
            List<SqlParameter> param = new List<SqlParameter>(){ };
            string queryName = "MOB_SALE_GetSaleInvoiceTypes";
            List<DropDown> response = FetchInvoiceTypes(queryName, param);
            return response;
        }
        private List<DropDown> FetchInvoiceTypes(string queryName, List<SqlParameter> param)
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
                            DropDown invoiceType = new DropDown();
                            invoiceType.Id = Convert.ToInt32(row["ID"] ?? 0);
                            invoiceType.Name = row["Name"].ToString() ?? "";
                            list.Add(invoiceType);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get Delivery Types
        public List<DropDown> GetDeliveryTypes()
        {
            List<SqlParameter> param = new List<SqlParameter>() { };
            string queryName = "MOB_SALE_GetSaleDeliveryTypes";
            List<DropDown> response = FetchDeliveryTypes(queryName, param);
            return response;
        }
        private List<DropDown> FetchDeliveryTypes(string queryName, List<SqlParameter> param)
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
                            DropDown invoiceType = new DropDown();
                            invoiceType.Id = Convert.ToInt32(row["ID"] ?? 0);
                            invoiceType.Name = row["Name"].ToString() ?? "";
                            list.Add(invoiceType);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get Sale Customer List
        public List<DropDown> GetSaleCustomers(int compnayId,string text)
        {
            List<SqlParameter> param = new List<SqlParameter>() 
            {
             new SqlParameter("@CompanyID",compnayId),
             new SqlParameter("@Text",text)
            };
            string queryName = "MOB_SALE_GetCustomerList";
            List<DropDown> response = FetchSaleCustomers(queryName, param);
            return response;
        }
        private List<DropDown> FetchSaleCustomers(string queryName, List<SqlParameter> param)
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
                            DropDown invoiceType = new DropDown();
                            invoiceType.Id = Convert.ToInt32(row["ID"] ?? 0);
                            invoiceType.Name = row["Name"].ToString() ?? "";
                            list.Add(invoiceType);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get Channels List
        public List<Channels> GetAllChannels()
        {
            List<SqlParameter> param = new List<SqlParameter>() { };
            string queryName = "MOB_GetActiveChannels";
            List<Channels> response = FetchAllChannels(queryName, param);
            return response;
        }
        private List<Channels> FetchAllChannels(string queryName, List<SqlParameter> param)
        {
            List<Channels> list = new List<Channels>();
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
                            Channels invoiceType = new Channels();
                            invoiceType.Id = Convert.ToInt32(row["ID"] ?? 0);
                            invoiceType.Name = row["Name"].ToString() ?? "";
                            list.Add(invoiceType);
                        }
                    }
                }
            }
            return list;
        }
        #endregion
        #region Get All Sale Status 
        public List<SaleStatus> GetAllSaleStatus()
        {
            List<SqlParameter> param = new List<SqlParameter>() { };
            string queryName = "MOB_SALE_GetSaleStatus";
            List<SaleStatus> response = FetchAllSaleStatus(queryName, param);
            return response;
        }
        private List<SaleStatus> FetchAllSaleStatus(string queryName, List<SqlParameter> param)
        {
            List<SaleStatus> list = new List<SaleStatus>();
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
                            SaleStatus invoiceType = new SaleStatus();
                            invoiceType.Id = Convert.ToInt32(row["ID"] ?? 0);
                            invoiceType.Name = row["Name"].ToString() ?? "";
                            list.Add(invoiceType);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Order Details
        public OrderDetails GetOrderDetails(long saleId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@SaleID",saleId)
            };
            string queryName = "MOB_SALE_GetOrderDetails";
            OrderDetails response = FetchOrderDetails(queryName, param);
            return response;
        }
        private OrderDetails FetchOrderDetails(string queryName, List<SqlParameter> param)
        {
            OrderDetails order = new OrderDetails();
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
                            order.SaleId = Convert.ToInt64(row["SaleID"] ?? 0);
                            order.RegisterDate = row["RegisterDate"].ToString() ?? "";
                            order.CustomerId = !string.IsNullOrEmpty(row["CustomerID"].ToString()) ? Convert.ToInt32(row["CustomerID"]) : default;
                            order.Customer = row["Customer"].ToString() ?? "";
                            order.Contact = row["Contact"].ToString() ?? "";
                            order.ExpectedDate = row["ExpectedDate"].ToString() ?? "";
                            order.Store = row["Store"].ToString() ?? "";
                            order.Email = row["Email"].ToString() ?? "";
                            order.Seller = row["Seller"].ToString() ?? "";
                            order.Warehouse = row["Warehouse"].ToString() ?? "";
                            order.Reference = row["Reference"].ToString() ?? "";
                            order.Phone = row["Phone"].ToString() ?? "";
                            order.PaymentTerm = row["PaymentTerm"].ToString() ?? "";
                            order.Status = row["PaymentTerm"].ToString() ?? "";
                            order.DeliveryType = row["DeliveryType"].ToString() ?? "";
                            order.PurchaseOrder = row["PurchaseOrder"].ToString() ?? "";
                            order.SubTotal =Convert.ToDecimal( row["SubTotal"] ?? 0);
                            order.Iva =Convert.ToDecimal( row["Iva"] ?? 0);
                            order.Ieps =Convert.ToDecimal( row["Ieps"] ?? 0);
                            order.Total =Convert.ToDecimal( row["Total"] ?? 0);
                        }
                    }
                }
            }
            return order;
        }
        #endregion

        #region Get Order Items List
        public List<OrderItem> GetOrderItemsList(long saleId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@SaleID",saleId)
            };
            string queryName = "MOB_SALE_GetOrderItems";
            List<OrderItem> response = FetchOrderItemsList(queryName, param);
            return response;
        }
        private List<OrderItem> FetchOrderItemsList(string queryName, List<SqlParameter> param)
        {
            List<OrderItem> list = new List<OrderItem>();
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
                            OrderItem item = new OrderItem();
                            item.SKU = row["SKU"].ToString() ?? "";
                            item.Total = row["Total"].ToString() ?? "";
                            item.Title = row["Title"].ToString() ?? "";
                            item.Warehouse = row["Warehouse"].ToString() ?? "";
                            item.Quantity = Convert.ToDecimal(row["Quantity"] ?? 0);
                            item.Price = Convert.ToDecimal(row["Price"] ?? 0);
                            item.Amount = Convert.ToDecimal(row["Amount"] ?? 0);
                            item.Iva = Convert.ToDecimal(row["Iva"] ?? 0);
                            item.Ieps = Convert.ToDecimal(row["Ieps"] ?? 0);
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Update Invoice Link
        public InvoiceResponse UpdateInvoicingLink(InvoiceRequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@SaleID",req.SaleId),
              new SqlParameter("@ExpirationDate",req.ExpiryDate)
            };
            string queryName = "MOB_SALE_UpdateInvoicingLink";
            InvoiceResponse response = FetchUpdateInvoicingLink(queryName, param);
            return response;
        }
        private InvoiceResponse FetchUpdateInvoicingLink(string queryName, List<SqlParameter> param)
        {
            InvoiceResponse result = new InvoiceResponse();
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
                            result.Success = row["Success"].ToString() ?? "";
                            result.Title = row["Title"].ToString() ?? "";
                            result.Description = row["Description"].ToString() ?? "";
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Update Order Status
        public string UpdateOrderStatus(StatusRequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@SaleID",req.SaleId),
              new SqlParameter("@StatusID",req.StatusId)
            };
            string queryName = "MOB_SALE_UpdateOrderStatus";
            string response = ExecuteNonQuery(_executeContext, queryName, param,true).ToString();
            return response;
        }
        #endregion

        #region Get Invoice Link
        public InvioiceLink GetInvoiceLink(int saleId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@SaleID",saleId)
            };
            string queryName = "MOB_SALE_GetInvoicingLink";
            InvioiceLink response = FetchInvoiceLink(queryName, param);
            return response;
        }
        private InvioiceLink FetchInvoiceLink(string queryName, List<SqlParameter> param)
        {
            InvioiceLink result = new InvioiceLink();
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
                            result.Folio = row["Folio"].ToString() ?? "";
                            result.Reference = row["Reference"].ToString() ?? "";
                            result.ExpirationDate = row["ExpirationDate"].ToString() ?? "";
                            result.ExpDays = Convert.ToInt32(row["ExpDays"] ?? 0);
                            result.LinkUrl = row["URL"].ToString() ?? "";
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region Cancel Sale Order
        public string CancelSaleOrder(CancelRequest req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@CompanyID",req.CompanyId),
              new SqlParameter("@SaleID",req.SaleId),
            };
            string queryName = "MOB_SALE_CancelSaleOrder";
            string response = commonDAO.FetchGenericColumn(queryName, param).ToString();
            return response;
        }
        #endregion

        #region Get Emisor List
        public List<Emisor> GetEmisorList(int companyId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@CompanyID",companyId)
            };
            string queryName = "MOB_SALE_GetEmisorList";
            List<Emisor> response = FetchEmisorList(queryName, param);
            return response;
        }
        private List<Emisor> FetchEmisorList(string queryName, List<SqlParameter> param)
        {
            List<Emisor> list = new List<Emisor>();
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
                            Emisor emisor = new Emisor();
                            emisor.ProfileNo = row["ProfileNo"].ToString() ?? "";
                            emisor.TaxName = row["TaxName"].ToString() ?? "";
                            emisor.TaxID = row["TaxID"].ToString() ?? "";
                            list.Add(emisor);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Uso De CFDI
        public List<DropDownV2> GetUsoDeCFDIList()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {

            };
            string queryName = "MOB_CAT_getusodecfdi";
            List<DropDownV2> response = FetchUsoDeCFDIList(queryName, param);
            return response;
        }
        private List<DropDownV2> FetchUsoDeCFDIList(string queryName, List<SqlParameter> param)
        {
            List<DropDownV2> list = new List<DropDownV2>();
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
                            DropDownV2 obj = new DropDownV2();
                            obj.Id = row["ID"].ToString() ?? "";
                            obj.Name = row["name"].ToString() ?? "";
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Payment Terms
        public List<DropDownV2> GetPaymentTerms()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              
            };
            string queryName = "MOB_CAT_getmetododepago";
            List<DropDownV2> response = FetchPaymentTerms(queryName, param);
            return response;
        }
        private List<DropDownV2> FetchPaymentTerms(string queryName, List<SqlParameter> param)
        {
            List<DropDownV2> list = new List<DropDownV2>();
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
                            DropDownV2 obj = new DropDownV2();
                            obj.Id = row["id"].ToString() ?? "";
                            obj.Name = row["name"].ToString() ?? "";
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Regimen Fiscals List
        public List<Regimen> GetRegimenFiscalList()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {

            };
            string queryName = "MOB_INV_GetRegimenFiscalList";
            List<Regimen> response = FetchRegimenFiscalList(queryName, param);
            return response;
        }
        private List<Regimen> FetchRegimenFiscalList(string queryName, List<SqlParameter> param)
        {
            List<Regimen> list = new List<Regimen>();
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
                            Regimen obj = new Regimen();
                            obj.RegimenFiscal = Convert.ToInt32(row["RegimenFiscal"] ?? 0);
                            obj.Nombre = row["Nombre"].ToString() ?? "";
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Payment Methods
        public List<DropDown> GetPaymentMethods()
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {

            };
            string queryName = "MOB_CAT_getformadepago";
            List<DropDown> response = FetchPaymentMethods(queryName, param);
            return response;
        }
        private List<DropDown> FetchPaymentMethods(string queryName, List<SqlParameter> param)
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
                            DropDown obj = new DropDown();
                            obj.Id = Convert.ToInt32(row["id"] ?? 0);
                            obj.Name = row["name"].ToString() ?? "";
                            list.Add(obj);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Create Request
        public string CreateRequest(RequestDTO req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
              new SqlParameter("@Channel",req.Channel),
              new SqlParameter("@ReportName",req.ReportName),
              new SqlParameter("@CompanyID",req.CompanyId),
              new SqlParameter("@UserID",req.UserMKTId),
              new SqlParameter("@MktAccountID",req.MKTAccountId),
              new SqlParameter("@MktAccountNickname",req.MKTAccountNickName),
              new SqlParameter("@Filters",req.Filters)
            };
            string queryName = "MOB_EXP_CreateRequest";
            string response = commonDAO.FetchGenericNoColumnName(queryName, param).ToString();
            return response;
        }
        #endregion
    }
}
