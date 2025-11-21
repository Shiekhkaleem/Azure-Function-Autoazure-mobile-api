using AutoAzureMob.Models.DTO.SaleDTO;
using AutoAzureMob.Models.Models.Sale;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoAzureMob.Models.Models.OmniChannel;

namespace AutoAzureMob.DAL.DAL
{
    public class OmnichannelDAO : BaseDAO
    {
        private readonly IConfiguration _config;
        public OmnichannelDAO(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
        }


        #region Get OmniChannel List
        public List<OmniChannel> GetOmnichannelList(OmniRequest req)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",req.CompanyId),
               new SqlParameter("@ListedIn",req.ListedIn),
               new SqlParameter("@SearchText",req.SearchText),
               new SqlParameter("@Page",req.Page)
            };
            string queryName = "MOB_PROD_GetProductsMktsV2";
            List<OmniChannel> response = FetchOmnichannelList(queryName, param);
            return response;
        }
        private List<OmniChannel> FetchOmnichannelList(string queryName, List<SqlParameter> param)
        {
            List<OmniChannel> list = new List<OmniChannel>();
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
                            OmniChannel omni = new OmniChannel();
                            omni.Product = Convert.ToInt32(row["productid"] ?? 0);
                            omni.Sku = row["Sku"].ToString() ?? "";
                            omni.Name = row["name"].ToString() ?? "";
                            omni.Brand = row["brand"].ToString() ?? "";
                            omni.Stock = Convert.ToInt32(row["Stock"] ?? 0);
                            omni.TotalCount = Convert.ToInt32(row["TotalCount"] ?? 0);
                            omni.Sincronizacion = row["Sincronizacion"].ToString() ?? "";
                            list.Add(omni);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Related Publicacions
        public List<ProductDetails> RelatedPublicacionsById(int productId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@ProductID",productId),
            };
            string queryName = "PROD_MKT_GetRelatedPublications";
            List<ProductDetails> response = FetchRelatedPublicacionsById(queryName, param);
            return response;
        }
        private List<ProductDetails> FetchRelatedPublicacionsById(string queryName, List<SqlParameter> param)
        {
            List<ProductDetails> list = new List<ProductDetails>();
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
                            ProductDetails product = new ProductDetails();
                            product.ItemId = row["ItemID"].ToString() ?? "";
                            product.VariationId = row["VariationID"].ToString() ?? "";
                            product.Channel = row["Channel"].ToString() ?? "";
                            product.ChannelId = Convert.ToInt32(row["ChannelID"] ?? 0);
                            product.UserName = row["UserName"].ToString() ?? "";
                            product.Sku = row["Sku"].ToString() ?? "";
                            product.Title = row["Title"].ToString() ?? "";
                            product.Stock = Convert.ToInt32(row["Stock"] ?? 0);
                            product.Price = Convert.ToDecimal(row["Price"] ?? 0);
                            product.ImageUrl = row["ImageURL"].ToString() ?? "";
                            product.PubUrl = row["PubURL"].ToString() ?? "";
                            list.Add(product);
                        }
                    }
                }
            }
            return list;
        }
        #endregion

        #region Get Product Info
        public ProductInfo GetProductInfo(int companyId,int productId)
        {
            List<SqlParameter> param = new List<SqlParameter>()
            {
               new SqlParameter("@CompanyID",companyId),
               new SqlParameter("@ProductID",productId),
            };
            string queryName = "PROD_MKT_LoadProductInfo";
            ProductInfo response = FetchProductInfo(queryName, param);
            return response;
        }
        private ProductInfo FetchProductInfo(string queryName, List<SqlParameter> param)
        {
            ProductInfo product = new ProductInfo();
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
                            product.Sku = row["Sku"].ToString() ?? "";
                            product.Brand = row["Brand"].ToString() ?? "";
                            product.Title = row["Title"].ToString() ?? "";
                            product.Stock = Convert.ToDecimal(row["Stock"] ?? 0);
                            product.SyncStocks = row["SyncStocks"].ToString() ?? "";
                            product.SyncPrices = row["SyncPrices"].ToString() ?? "";
                            product.Prices = row["Prices"].ToString() ?? "";
                        }

                    }
                }

            }
            return product;
        }
        #endregion
    }
}
