using AutoAzureMob.BLL.Utils;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Balance;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.VM.Balance;
using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AzureUrl = AutoAzureMob.QueryResource.AzureUrls;
using AzureUrlProd = AutoAzureMob.QueryResource.AzureUrlsProd;

namespace AutoAzureMob.BLL.BLL
{
    public class BalanceHandler : BaseHandler
    {
        private readonly IConfiguration _config;
        private readonly BalanceDAO balanceDAO;
        private static bool LiveServer = false;
        private static string BlobUrl = string.Empty;
        public BalanceHandler(ExecuteContext executeContext, IConfiguration config) : base(executeContext, config)
        {
            _config = config;
            balanceDAO = new BalanceDAO(executeContext,_config);
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
            BlobUrl = _config.GetSection("BlobBaseUrl:Url").Value;
        }

        #region Get Card Payment Order
        public ResponseModel<PaymentOrderVM> GetCardPaymentOrder(int companyId, int page)
        {
            ResponseModel<PaymentOrderVM> response = new ResponseModel<PaymentOrderVM>();
            response.Content = new PaymentOrderVM();
            response.Content.PaymentOrders = balanceDAO.GetPaymentOrderList(companyId,page);
            response.Content.Card = balanceDAO.GetCurrentCardBalance(companyId);
            response.Title = "Extraviado";
            response.Description = "Registro no encontrado";
            response.Success = true;
            if (response.Content.PaymentOrders !=null && response.Content.PaymentOrders.Count > 0)
            {
                response.Description = "View model of both payment orders list and card current balance object";
                response.Title = "Success";
            }
            return response;
        }
        #endregion

        #region Get Export List
        public ResponseModel<List<Export>> GetExportList(int userId, int page)
        {
            ResponseModel<List<Export>> response = new ResponseModel<List<Export>>();
            response.Content = balanceDAO.GetExportList(userId, page); 
            response.Title = "Extraviado";
            response.Description = "Registro no encontrado";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
                foreach (var item in response.Content)
                {
                    dynamic output = JsonConvert.DeserializeObject(item.OutPut);
                    string PdfUrl = output[0].success;
                    var filtro = JsonConvert.DeserializeObject<BalanceResponse>(item.Filtro);
                    string CompanyId = filtro.CompanyID;
                    List<string> list = filtro.SaleIds;
               
                }
                response.Content.ForEach(x =>
                {
                    dynamic output = JsonConvert.DeserializeObject(x.OutPut);
                    x.PdfUrl = output[0].success;
                    var filtro = JsonConvert.DeserializeObject<BalanceResponse>(x.Filtro);
                    x.CompanyId = filtro.CompanyID;
                     x.SaleIds = filtro.SaleIds;
                });
                response.Description = "Exports list";
                response.Title = "Success";
            }
            return response;
        }
        #endregion

        #region Get Import List
        public ResponseModel<List<Import>> GetImportList(int userId, int page)
        {
            ResponseModel<List<Import>> response = new ResponseModel<List<Import>>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = balanceDAO.GetImportList(userId, page);
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            response.Title = "Extraviado";
            response.Description = "Registro no encontrado";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {               
                response.Description = "Imports list";
                response.Content.ForEach(e => {
                    e.ChannelThumbnail = logos.FirstOrDefault(x => x.Id == e.ChannelId).Url;
                    e.Estado = e.EstadoValue.Split(',').First();
                    e.EstadoValidar =e.EstadoValue.Split(',').ToList().Count > 1 ? textInfo.ToTitleCase( e.EstadoValue.Split(',').Last()) : string.Empty;
                });
                response.Title = "Success";
            }
            return response;
        }
        #endregion

        #region Get User Account List
        public ResponseModel<List<UserAccount>> GetUserAccountList()
        {
            ResponseModel<List<UserAccount>> response = new ResponseModel<List<UserAccount>>();
            string url = LiveServer ? AzureUrlProd.UserAccounts : AzureUrl.UserAccounts;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "GET";
            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            if (httpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                using (Stream stream = httpWebResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string json = reader.ReadToEnd();
                        response.Content = JsonConvert.DeserializeObject<List<UserAccount>>(json);
                        response.Content.ForEach(x =>
                        {
                           x.Foto = BlobUrl + x.Foto;
                        });
                    }
                }
            }
            response.Title = "Extraviado";
            response.Description = "Registro no encontrado";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Users account list";
                response.Title = "Success";
                response.Success = true;
            }
            return response;
        }
        #endregion

        #region Download Saldo Pdf and Xml
        public ResponseModel<string> DownloadSaldoXml(int invoiceId)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer ? AzureUrlProd.SaldoXML : AzureUrl.SaldoXML;
            string json = JsonConvert.SerializeObject(new
            {
                invoiceid = invoiceId,
                companyid = -2147483593
            });
            response.Description = "Xml base64 string not found.";
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (HttpResponseMessage httpResponse = httpClient.SendAsync(httpRequest).Result)
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            byte[] fileBytes = httpResponse.Content.ReadAsByteArrayAsync().Result;
                            response.Content = Convert.ToBase64String(fileBytes);
                            response.Success = true;
                            response.Description = "Xml base64 string.";
                        }
                    }
                }
            }
            return response;
        }

        public ResponseModel<string> DownloadSaldoPdf(int invoiceId)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer ? AzureUrlProd.SaldoPDF : AzureUrl.SaldoPDF;
            url = url + invoiceId;
            response.Description = "Pdf base64 string not found.";
            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, url))
                {

                    using (HttpResponseMessage httpResponse = httpClient.SendAsync(httpRequest).Result)
                    {
                        if (httpResponse.IsSuccessStatusCode)
                        {
                            byte[] fileBytes = httpResponse.Content.ReadAsByteArrayAsync().Result;
                            response.Content = Convert.ToBase64String(fileBytes);
                            response.Success = true;
                            response.Description = "Pdf base64 string.";
                        }
                    }
                }
            }
            return response;
        }
        #endregion

        #region Get Pagar Url
        public ResponseModel<string> GetPagarUrl(string companyId, string orderId, string redirectUrl)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer? AzureUrlProd.GetPagoURL : AzureUrl.GetPagoURL;
            string json = JsonConvert.SerializeObject(new
            {
                CompanyID = companyId,
                OrderID = orderId ,
                RedirectURL = redirectUrl
            });
            response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<string>>(url, json);
            if (response.Success)
            {
                response.Title = "Pagar button link.";
                response.Description = "Pagar button website url.";
            }
            return response;
        }
        #endregion
    }
}
