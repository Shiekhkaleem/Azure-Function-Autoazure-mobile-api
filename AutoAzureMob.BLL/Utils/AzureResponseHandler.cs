using AutoAzureMob.Models.Models;
using AutoAzureMob.Models.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.Utils
{
    public static class AzureResponseHandler
    {
        public static T GetAzureResponseObject<T>(string url, string json)
        {
            T serverObject = (T)Convert.ChangeType(null, typeof(T));
            AzureModel model = new AzureModel()
            {
                URL = url,
                JSON = json
            };

            string serverResponse = JsonResponse.GetJsonResponse(model);
            if (!string.IsNullOrWhiteSpace(serverResponse))
            {
                try
                {
                    serverObject = JsonConvert.DeserializeObject<T>(serverResponse);
                }
                catch (Exception ex)
                {
                }
            }

            return serverObject;
        }

        public static ResponseModel<List<T>> GetAzureResponseList<T>(string url, string json)
        {
            ResponseModel<List<T>> listObject = (ResponseModel<List<T>>)Convert.ChangeType(null, typeof(T));
            AzureModel model = new AzureModel()
            {
                URL = url,
                JSON = json,
                RequestType = !string.IsNullOrWhiteSpace(json) ? "POST" : "GET"
            };

            string serverResponse = JsonResponse.GetJsonResponse(model);
            if (!string.IsNullOrWhiteSpace(serverResponse))
            {
                try
                {
                    listObject = JsonConvert.DeserializeObject<ResponseModel<List<T>>>(serverResponse);
                }
                catch (Exception ex)
                {
                }
            }

            return listObject;
        }
    }
}
