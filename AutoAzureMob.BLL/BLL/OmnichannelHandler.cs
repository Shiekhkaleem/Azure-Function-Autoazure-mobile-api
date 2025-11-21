using AutoAzureMob.BLL.Utils;
using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.DTO;
using AutoAzureMob.Models.DTO.OmniDTO;
using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.OmniChannel;
using AutoAzureMob.Models.Models.Response;
using AutoAzureMob.Models.Models.Sale;
using AutoAzureMob.Models.VM.OmniChannelVM;
using AutoAzureMob.Models.VM.ProductVM;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureUrl = AutoAzureMob.QueryResource.AzureUrls;
using AzureUrlProd = AutoAzureMob.QueryResource.AzureUrlsProd;

namespace AutoAzureMob.BLL.BLL
{
    public class OmnichannelHandler : BaseHandler
    {
        private readonly IConfiguration config;
        private readonly OmnichannelDAO omnichannelDAO;
        private readonly SaleHandler saleHandler;
        private static bool LiveServer = false;
        public OmnichannelHandler(ExecuteContext executeContext, IConfiguration _config) : base(executeContext, _config)
        {
            config = _config;
            omnichannelDAO = new OmnichannelDAO(executeContext, config);
            saleHandler = new SaleHandler(executeContext, config);
            LiveServer = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production");
        }

        #region Get OmniChannel List

        public ResponseModel<List<OmniChannel>> GetOmniChannelList(OmniRequest req)
        {
            ResponseModel<List<OmniChannel>> response = new ResponseModel<List<OmniChannel>>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            response.Content = omnichannelDAO.GetOmnichannelList(req);
            response.Description = "Lista OminiChannel no encontrada";
            response.Title = "Extraviado";
            response.Success = true;
            if (response.Content != null && response.Content.Count > 0)
            {
               response.Content.ForEach(x =>
                {
                    if (x.Sincronizacion.Any())
                    {
                        List<RootObjectDTO> canalsList = JsonConvert.DeserializeObject<List<RootObjectDTO>>(x.Sincronizacion);
                        canalsList.ForEach(y =>
                        {
                            List<Canal> list = JsonConvert.DeserializeObject<List<Canal>>(y.Canal);
                            list.ForEach(item => { x.Canales.Add(item); });
                            x.Canales.ForEach(z => { z.Thumbnail = logos.FirstOrDefault(a => a.Id == Convert.ToInt32(z.ID)).Url; });
                            x.TotalCanals = x.Canales.Count();
                            x.TotalPrice = x.Canales.Count(x => x.SyncPrice == true);
                            x.TotalStock = x.Canales.Count(x => x.SyncStock == true);
                        });
                    }
                });
                response.Description = "OminiLista de canales";
                response.Title = "Encontró";
            }
            return response;
        }
        #endregion

        #region Get Related Publicacions
        public ResponseModel<List<ProductDetailsVM>> RelatedPublicacionsById(int productId)
        {
            ResponseModel<List<ProductDetailsVM>> response = new ResponseModel<List<ProductDetailsVM>>();
            response.Content = new List<ProductDetailsVM>();
            string json = File.ReadAllText("Logos.json");
            List<Logos> logos = new List<Logos>();
            logos = JsonConvert.DeserializeObject<List<Logos>>(json);
            List<ProductDetails> products = omnichannelDAO.RelatedPublicacionsById(productId);
            response.Description = "Publicaciones relacionadas no encontradas";
            response.Title = "Extraviado";
            response.Success = true;
            if (products != null && products.Count > 0)
            {
                var groupBy = products.GroupBy(x => x.ChannelId);
                foreach (var item in groupBy)
                {
                    ProductDetailsVM vm = new ProductDetailsVM()
                    {
                        ChannelId = item.Key,
                        ChannelName = item.FirstOrDefault().Channel,
                        Thumbnail = logos.FirstOrDefault(y => y.Id == item.Key).Url,
                        ProductDetail = products.Where(y => y.ChannelId == item.Key).ToList()
                    };
                    response.Content.Add(vm);
                }
                //products.ForEach(x =>
                //{
                //    x.Thumbnail = logos.FirstOrDefault(y => y.Id == x.ChannelId).Url;
                //    ProductDetailsVM vm = new ProductDetailsVM()
                //    {
                //        ChannelId = x.ChannelId,
                //        ChannelName = x.Channel,
                //        Thumbnail = x.Thumbnail,
                //        ProductDetail = products.Where(y => y.ChannelId == x.ChannelId).ToList()
                //    };
                //    response.Content.Add(vm);
                //});
                response.Description = "Lista de publicaciones relacionadas";
                response.Title = "Encontró";
            }
            return response;
        }
        #endregion

        #region Update Sync Stock And Price 
        public ResponseModel<string> UpdateSyncStockPrice(OmniRequestDTO req)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            string url = LiveServer ? AzureUrlProd.UpdateSyncStockPrices : AzureUrl.UpdateSyncStockPrices;
            StockPriceRequestVM requestVM = new();
            requestVM.SyncStocks.Add(new StockPrice
            {
                Name = "SyncStock",
                ChannelID = req.ChannelId.ToString(),
                value = req.SyncStocks
            });
            requestVM.SyncPrices.Add(new StockPrice
            {
                Name = "SyncPrices",
                ChannelID = req.ChannelId.ToString(),
                value = req.SyncPrices
            });
            string json = JsonConvert.SerializeObject(new
            {
                CompanyID = req.CompanyId,
                ProductID = req.ProductId,
                SyncStocks = requestVM.SyncStocks,
                SyncPrices = requestVM.SyncPrices,
                Prices = req.Prices
            });
            ResponseModel<string> result = AzureResponseHandler.GetAzureResponseObject<ResponseModel<string>>(url, json);
            if (result != null)
            {
                response.Success = true;
                response.Title = result.Title;
                response.Description = result.Description;
            }
            return response;
        }
        #endregion

        #region Get Product Info
        public ResponseModel<StockPriceResponseVM> GetProductInfo(int companyId, int productId)
        {
            ResponseModel<StockPriceResponseVM> response = new ResponseModel<StockPriceResponseVM>();
            response.Content = new StockPriceResponseVM();
            ProductInfo productInfo = omnichannelDAO.GetProductInfo(companyId, productId);
            ResponseModel<List<Channels>> channelsResponse = saleHandler.GetAllChannels();
            response.Description = "Información del producto no encontrada";
            response.Title = "Extraviado";
            response.Success = true;
            if (productInfo != null)
            {
                var syncStocks = JsonConvert.DeserializeObject<List<StockPrice>>(productInfo.SyncStocks);
                var syncPrice = JsonConvert.DeserializeObject<List<StockPrice>>(productInfo.SyncPrices);
                var prices = JsonConvert.DeserializeObject<List<Price>>(productInfo.Prices);
                var stockList = syncStocks.Select(x => new StockPriceVM
                {
                    ChannelId = Convert.ToInt32(x.ChannelID),
                }).ToList();
                stockList.ForEach(x =>
                {
                    x.ChannelId = x.ChannelId;
                    x.SyncStocks = syncStocks.FirstOrDefault(y => Convert.ToInt32(y.ChannelID) == x.ChannelId).value;
                    x.SyncPrices = syncPrice.FirstOrDefault(z => Convert.ToInt32(z.ChannelID) == x.ChannelId).value;
                    x.Prices = prices.Where(a => Convert.ToInt32(a.ChannelID) == x.ChannelId).ToList();
                });
                response.Content = new()
                {
                    Sku = productInfo.Sku,
                    Title = productInfo.Title,
                    Brand = productInfo.Brand,
                    Stock = productInfo.Stock,
                    StockPriceVM = stockList,
                    Channels = channelsResponse.Content != null && channelsResponse.Content.Count > 0 ? channelsResponse.Content : new(),
                };
                response.Description = "Información del producto";
                response.Title = "Encontró";
            }
            return response;
        }
        #endregion
    }
}
