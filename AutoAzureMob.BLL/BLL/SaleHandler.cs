using AutoAzureMob.BLL.Utils;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.SaleDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.Sale;
using AutoAzureMob.Models.VM.SaleVM;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AzureUrl = AutoAzureMob.QueryResource.AzureUrls;
using AzureUrlProd = AutoAzureMob.QueryResource.AzureUrlsProd;

namespace AutoAzureMob.BLL.BLL
{
    public class SaleHandler  :BaseHandler
    {
        private readonly SaleDAO saleDAO ;
        private readonly NotificationDAO notificationDAO;
        private readonly IConfiguration config;
        private static bool LiveServer = false;
        public SaleHandler(ExecuteContext executeContext, IConfiguration _config) : base(executeContext, _config)
        {
            config = _config;
            saleDAO = new SaleDAO(executeContext, config);
            notificationDAO = new NotificationDAO(executeContext, config);
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
        }
      
        public ResponseModel<List<SaleOrders>> GetSaleOrders(OrderRequestDTO req)
        {
            ResponseModel<List<SaleOrders>> response = new ResponseModel<List<SaleOrders>>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            if ( req.Channels != null)
            {
                StringBuilder stringBuilder = new StringBuilder();
                var channels = req.Channels.Split(',');
                for (int i = 0; i < channels.Length; i++)
                {
                    stringBuilder.Append("<" + channels[i] + ">");
                    if (i < channels.Length -1)
                    {
                        stringBuilder.Append(",");
                    }
                }
                req.Channels = stringBuilder.ToString();
            }
            response.Content = saleDAO.GetSaleOrders(req);
            response.Success = true;
            response.Description = "No hay ordenes contra esta empresa";
            response.Title = "Extraviado";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "No hay ordenes contra esta empresa";
                response.Content.ForEach(e => {
                    e.ChannelThumbnail = logos.FirstOrDefault(y => y.Id == e.ChannelId).Url;
                });
            }
            return response;
        }

        public ResponseModel<List<DropDown>> GetInvoiceTypes()
        {
            ResponseModel<List<DropDown>> response = new ResponseModel<List<DropDown>>();
            response.Content = saleDAO.GetInvoiceTypes();
            response.Success = true;
            response.Description = "Invoice Types not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Invoice Types drop down list";
            }
            return response;
        }

        public ResponseModel<List<DropDown>> GetDeliveryTypes()
        {
            ResponseModel<List<DropDown>> response = new ResponseModel<List<DropDown>>();
            response.Content = saleDAO.GetDeliveryTypes();
            response.Success = true;
            response.Description = "Delivery Types not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Delivery Types drop down list";
            }
            return response;
        }

        public ResponseModel<List<DropDown>> GetSaleCustomers(int compnayId, string text)
        {
            ResponseModel<List<DropDown>> response = new ResponseModel<List<DropDown>>();
            response.Content = saleDAO.GetSaleCustomers(compnayId,text);
            response.Success = true;
            response.Description = "Sale customers not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Sale customers drop down list";
            }
            return response;
        }

        public ResponseModel<List<Channels>> GetAllChannels()
        {
            ResponseModel<List<Channels>> response = new ResponseModel<List<Channels>>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = saleDAO.GetAllChannels();
            response.Success = true;
            response.Description = "Channels not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Content.ForEach(e => { e.Thummbnail = logos.FirstOrDefault(x => x.Id == e.Id).Url;  });
                response.Description = "Channels list";
            }
            return response;
        }

        public ResponseModel<List<SaleStatus>> GetAllSaleStatus()
        {
            ResponseModel<List<SaleStatus>> response = new ResponseModel<List<SaleStatus>>();
            string json = File.ReadAllText("JsonData\\StatusColors.json");
            List<Logos> codes = new List<Logos>();
            codes = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = saleDAO.GetAllSaleStatus();
            response.Success = true;
            response.Description = "Sale status list not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Sale status list";
                response.Content.ForEach(e => { e.Code = codes.FirstOrDefault(x => x.Id == e.Id).Url; });
            }
            return response;
        }

        public ResponseModel<FilterPageLoadVM> GetFilterPageLoadVM()
        {
            ResponseModel<FilterPageLoadVM> response = new ResponseModel<FilterPageLoadVM>();
            response.Content = new FilterPageLoadVM();
            string json = File.ReadAllText("JsonData\\StatusColors.json");
            List<Logos> codes = new List<Logos>();
            codes = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content.SaleStatus = saleDAO.GetAllSaleStatus();
            string jsons = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(jsons);
            response.Content.Channels = saleDAO.GetAllChannels();
            response.Success = true;
            response.Description = "Sale Filter Page Load Not Found.";
            if (response.Content != null && response.Content.SaleStatus.Count > 0)
            {
                response.Description = "Sale Filter Page Load.";
                response.Content.SaleStatus.ForEach(e => { e.Code = codes.FirstOrDefault(x => x.Id == e.Id).Url; });
                response.Content.Channels.ForEach(e => { e.Thummbnail = logos.FirstOrDefault(x => x.Id == e.Id).Url; });
            }
            return response;
        }

        public ResponseModel<OrderDetails> GetOrderDetails(long saleId)
        {
            ResponseModel<OrderDetails> response = new ResponseModel<OrderDetails>();
            response.Content = saleDAO.GetOrderDetails(saleId);
            response.Success = true;
            response.Description = "Sale details not found";
            if (response.Content != null)
            {
                response.Description = "Sale details";
            }
            return response;
        }

        public ResponseModel<List<OrderItem>> GetOrderItemsList(long saleId)
        {
            ResponseModel<List<OrderItem>> response = new ResponseModel<List<OrderItem>>();
            response.Content = saleDAO.GetOrderItemsList(saleId);
            response.Success = true;
            response.Description = "Order items list not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Order items list";
            }
            return response;
        }

        public ResponseModel<InvoiceResponse> UpdateInvoicingLink(InvoiceRequestDTO req)
        {
            ResponseModel<InvoiceResponse> response = new ResponseModel<InvoiceResponse>();
            response.Content = saleDAO.UpdateInvoicingLink(req);
            response.Success = response.Content.Success.Equals("true") ? true : false;
            response.Title = response.Content.Title;
            response.Description = response.Content.Description;    
            return response;
        }

        public ResponseModel<string> UpdateOrderStatus(StatusRequestDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            response.Content = saleDAO.UpdateOrderStatus(req);
            response.Title = "Sin actualizar";
            response.Description = "Wstado del pedido no actualizado";
            if (response.Content == "1")
            {
                response.Success = true;
                response.Title = "Actualizada";
                response.Description = "Estado del pedido actualizado con éxito";
            }
            return response;
        }

        public ResponseModel<InvioiceLink> GetInvoiceLink(int saleId)
        {
            ResponseModel<InvioiceLink> response = new ResponseModel<InvioiceLink>();
            response.Content = saleDAO.GetInvoiceLink(saleId);
            response.Title = "extraviado";
            response.Description = "Enlace de factura no encontrado";
            if (!string.IsNullOrEmpty(response.Content.LinkUrl))
            {
                response.Success = true;
                response.Title = "Encontrada";
                response.Description = "enlace de factura";
            }
            return response;
        }

        public ResponseModel<string> CancelSaleOrder(List<CancelRequest> req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            foreach (var item in req)
            {
                response.Content = saleDAO.CancelSaleOrder(item);
            }
            response.Success = true;
            response.Description = !string.IsNullOrEmpty(response.Content)? response.Content : "Pedidos no cancelados.";
            response.Title = !string.IsNullOrEmpty(response.Content)? "éxito" : "fallida";
            return response;
        }

        public ResponseModel<List<Emisor>> GetEmisorList(int companyId)
        {
            ResponseModel<List<Emisor>> response = new ResponseModel<List<Emisor>>();
            response.Content = saleDAO.GetEmisorList(companyId);
            response.Success = true;
            response.Description = "Emisor list not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Emisor list";
            }
            return response;
        }

        public ResponseModel<FacturaPageLoadVM> GetFacturaPageLoadVM(int companyId)
        {
            ResponseModel<FacturaPageLoadVM> response = new ResponseModel<FacturaPageLoadVM>();
            response.Content = new FacturaPageLoadVM();
            response.Content.RegimenFiscalList = saleDAO.GetRegimenFiscalList();
            response.Content.EmisorList = saleDAO.GetEmisorList(companyId);
            response.Content.UsoDeCFDIList = saleDAO.GetUsoDeCFDIList();
            response.Content.FormaDePagoList = saleDAO.GetPaymentMethods();
            response.Content.MetodoDePagoList = saleDAO.GetPaymentTerms();
            response.Success = true;
            response.Description = "Factura Page Load VM not found";
            if (response.Content != null)
            {
                response.Description = "Factura Page Load View Model";
            }
            return response;
        }

        public ResponseModel<List<DropDownV2>> GetUsoDeCFDIList()
        {
            ResponseModel<List<DropDownV2>> response = new ResponseModel<List<DropDownV2>>();
            response.Content = saleDAO.GetUsoDeCFDIList();
            response.Success = true;
            response.Description = "Uso De CFDI List not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Uso De CFDI drop down list";
            }
            return response;
        }

        public ResponseModel<List<DropDownV2>> GetPaymentTerms()
        {
            ResponseModel<List<DropDownV2>> response = new ResponseModel<List<DropDownV2>>();
            response.Content = saleDAO.GetPaymentTerms();
            response.Success = true;
            response.Description = "Payment terms not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Payment terms";
            }
            return response;
        }

        public ResponseModel<List<Regimen>> GetRegimenFiscalList()
        {
            ResponseModel<List<Regimen>> response = new ResponseModel<List<Regimen>>();
            response.Content = saleDAO.GetRegimenFiscalList();
            response.Success = true;
            response.Description = "Regimen Fiscal List not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Regimen Fiscal List";
            }
            return response;
        }

        public ResponseModel<List<DropDown>> GetPaymentMethods()
        {
            ResponseModel<List<DropDown>> response = new ResponseModel<List<DropDown>>();
            response.Content = saleDAO.GetPaymentMethods();
            response.Success = true;
            response.Description = "Payment methods not found";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Payment methods drop down list";
            }
            return response;
        }

        public ResponseModel<string> CreateRequest(List<RequestDTO> req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            if (req != null && req.Count > 0)
            {
                foreach (var item in req)
                {
                    if (item.SaleId.Any())
                    {
                        item.Filters = "{\"CompanyID\":\"{companyId}\",\"SaleIds\":[\"{saleId}\"]}";
                        item.Filters = item.Filters.Replace("{companyId}", item.CompanyId.ToString()).Replace("{saleId}", item.SaleId);
                    }
                    response.Content = saleDAO.CreateRequest(item);
                    response.Title = "Solicitud enviada";
                    response.Success = true;
                    response.Description = response.Content;
                }
            }
            
            return response;
        }

        public ResponseModel<string> FacturarRemission(RemissionRequest req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer ? AzureUrlProd.FacrurarRemission : AzureUrl.FacrurarRemission;
            string json = JsonConvert.SerializeObject(new
            {
              Type=req.Type,
              UserID = req.UserId ,
              CompanyID =req.CompanyId,
              SaleID =req.SaleId,
              PayForm  =req.PayForm,
              PayMethod= req.PayMethod,
              CustomerID =req.CustomerId,
              UseOfCFDI = req.UseOfCFDI,
              PayConditions = req.PayConditions,
              Email = req.Email,
              Bank = req.Bank,
              PayReference = req.PayReference,
              TaxName = req.TaxName,
              TaxID = req.TaxId,
              Street = req.Street,
              NoExt = req.NoExt,
              NoInt = req.NoInt,
              Neighborhood = req.Neighborhood,
              City = req.City,
              State = req.State,
              Date = req.Date,
              ZipCode = req.ZipCode,
              RegimenFiscal = req.RegimenFiscal,
              ProfileNo = req.ProfileNo
        });
            response = AzureResponseHandler.GetAzureResponseObject<ResponseModel<string>>(url, json);
            return response;
        }                                                                                       
    }
}
