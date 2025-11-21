using System.Net;
using AutoAzureMob.API.Helper;
using AutoAzureMob.BLL.BLL;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO.SaleDTO;
using AutoAzureMob.Models.Models.Company;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.User;
using AutoAzureMob.Models.VM.SaleVM;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace AutoAzureMob.API.Functions
{
    public class SaleFunction
    {
        private readonly ILogger _logger;
        private readonly IConfiguration config;
        private readonly ExecuteContext executecontext;
        private readonly SaleHandler saleHandler;
        public SaleFunction(ILoggerFactory loggerFactory, ExecuteContext _executecontext = null)
        {
            _logger = loggerFactory.CreateLogger<SaleFunction>();
            executecontext = _executecontext;
            config = ConfigurationHelper.GetConfiguration();
            saleHandler = new SaleHandler(executecontext, config);
        }

        [Function("GetSaleOrders")]
        [OpenApiOperation(operationId: "GetSaleOrders", tags: new[] { "Sales" })]
        [OpenApiRequestBody("application/json", typeof(OrderRequestDTO))]
        public async Task<HttpResponseData> GetSaleOrders([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetSaleOrders");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderRequestDTO data = JsonConvert.DeserializeObject<OrderRequestDTO>(requestBody);
            var result = saleHandler.GetSaleOrders(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetInvoiceTypes")]
        [OpenApiOperation(operationId: "GetInvoiceTypes", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetInvoiceTypes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetInvoiceTypes");
        
            var result = saleHandler.GetInvoiceTypes();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetDeliveryTypes")]
        [OpenApiOperation(operationId: "GetDeliveryTypes", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetDeliveryTypes([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetDeliveryTypes");

            var result = saleHandler.GetDeliveryTypes();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetSaleCustomers")]
        [OpenApiOperation(operationId: "GetSaleCustomers", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        [OpenApiParameter(name: "Text", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "")]
        public async Task<HttpResponseData> GetSaleCustomers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetSaleCustomers");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int compnayId =Convert.ToInt32( queryParam["CompanyId"]);
            string text = queryParam["Text"];
            var result = saleHandler.GetSaleCustomers(compnayId,text);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetAllChannels")]
        [OpenApiOperation(operationId: "GetAllChannels", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetAllChannels([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetAllChannels");

            var result = saleHandler.GetAllChannels();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetAllSaleStatus")]
        [OpenApiOperation(operationId: "GetAllSaleStatus", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetAllSaleStatus([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetAllSaleStatus");

            var result = saleHandler.GetAllSaleStatus();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetFilterPageLoadVM")]
        [OpenApiOperation(operationId: "GetFilterPageLoadVM", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetFilterPageLoadVM([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetFilterPageLoadVM");

            var result = saleHandler.GetFilterPageLoadVM();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetOrderDetails")]
        [OpenApiOperation(operationId: "GetOrderDetails", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "SaleId", In = ParameterLocation.Query, Required = true, Type = typeof(long), Description = "")]
       
        public async Task<HttpResponseData> GetOrderDetails([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetOrderDetails");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            long saleId = Convert.ToInt64(queryParam["SaleId"]);
            var result = saleHandler.GetOrderDetails(saleId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetOrderItemsList")]
        [OpenApiOperation(operationId: "GetOrderItemsList", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "SaleId", In = ParameterLocation.Query, Required = true, Type = typeof(long), Description = "")]

        public async Task<HttpResponseData> GetOrderItemsList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetOrderItemsList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            long saleId = Convert.ToInt64(queryParam["SaleId"]);
            var result = saleHandler.GetOrderItemsList(saleId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateInvoicingLink")]
        [OpenApiOperation(operationId: "UpdateInvoicingLink", tags: new[] { "Sales" })]
        [OpenApiRequestBody("application/json", typeof(InvoiceRequestDTO))]
        public async Task<HttpResponseData> UpdateInvoicingLink([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("UpdateInvoicingLink");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            InvoiceRequestDTO data = JsonConvert.DeserializeObject<InvoiceRequestDTO>(requestBody);

            var result = saleHandler.UpdateInvoicingLink(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("UpdateOrderStatus")]
        [OpenApiOperation(operationId: "UpdateOrderStatus", tags: new[] { "Sales" })]
        [OpenApiRequestBody("application/json", typeof(StatusRequestDTO))]
        public async Task<HttpResponseData> UpdateOrderStatus([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("UpdateOrderStatus");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            StatusRequestDTO data = JsonConvert.DeserializeObject<StatusRequestDTO>(requestBody);

            var result = saleHandler.UpdateOrderStatus(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetInvoiceLink")]
        [OpenApiOperation(operationId: "GetInvoiceLink", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "SaleId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetInvoiceLink([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetInvoiceLink");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int saleId = Convert.ToInt32(queryParam["SaleId"]);
            var result = saleHandler.GetInvoiceLink(saleId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("CancelSaleOrder")]
        [OpenApiOperation(operationId: "CancelSaleOrder", tags: new[] { "Sales" })]
        [OpenApiRequestBody("application/json", typeof(List<CancelRequest>))]
        public async Task<HttpResponseData> CancelSaleOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("CancelSaleOrder");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<CancelRequest> data = JsonConvert.DeserializeObject<List<CancelRequest>>(requestBody);

            var result = saleHandler.CancelSaleOrder(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetEmisorList")]
        [OpenApiOperation(operationId: "GetEmisorList", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetEmisorList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetEmisorList");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int saleId = Convert.ToInt32(queryParam["CompanyId"]);
            var result = saleHandler.GetEmisorList(saleId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetFacturaPageLoadVM")]
        [OpenApiOperation(operationId: "GetFacturaPageLoadVM", tags: new[] { "Sales" })]
        [OpenApiParameter(name: "CompanyId", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "")]
        public async Task<HttpResponseData> GetFacturaPageLoadVM([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetFacturaPageLoadVM");
            var queryParam = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            int companyId = Convert.ToInt32(queryParam["CompanyId"]);
            ResponseModel<FacturaPageLoadVM> result = saleHandler.GetFacturaPageLoadVM(companyId);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetUsoDeCFDIList")]
        [OpenApiOperation(operationId: "GetUsoDeCFDIList", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetUsoDeCFDIList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetUsoDeCFDIList");

            var result = saleHandler.GetUsoDeCFDIList();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetPaymentTerms")]
        [OpenApiOperation(operationId: "GetPaymentTerms", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetPaymentTerms([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetPaymentTerms");
            
            var result = saleHandler.GetPaymentTerms();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetRegimenFiscalList")]
        [OpenApiOperation(operationId: "GetRegimenFiscalList", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetRegimenFiscalList([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetRegimenFiscalList");

            var result = saleHandler.GetRegimenFiscalList();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("GetPaymentMethods")]
        [OpenApiOperation(operationId: "GetPaymentMethods", tags: new[] { "Sales" })]
        public async Task<HttpResponseData> GetPaymentMethods([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("GetPaymentMethods");

            var result = saleHandler.GetPaymentMethods();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("CreateRequest")]
        [OpenApiOperation(operationId: "CreateRequest", tags: new[] { "Sales" })]
        [OpenApiRequestBody("application/json", typeof(List<RequestDTO>))]
        public async Task<HttpResponseData> CreateRequest([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("CreateRequest");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<RequestDTO> data = JsonConvert.DeserializeObject<List<RequestDTO>>(requestBody);
            var result = saleHandler.CreateRequest(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }

        [Function("FacturarRemission")]
        [OpenApiOperation(operationId: "FacturarRemission", tags: new[] { "Sales" })]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiSecurity("jwt_token", SecuritySchemeType.ApiKey, Name = "Authorization", In = OpenApiSecurityLocationType.Header, Scheme = OpenApiSecuritySchemeType.Bearer)]
        [OpenApiRequestBody("application/json", typeof(RemissionRequest))]
        public async Task<HttpResponseData> FacturarRemission([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("CreateRequest");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            RemissionRequest data = JsonConvert.DeserializeObject<RemissionRequest>(requestBody);
            var result = saleHandler.FacturarRemission(data);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(result);

            return response;
        }
    }
}
