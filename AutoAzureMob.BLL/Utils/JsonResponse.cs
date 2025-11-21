using AutoAzureMob.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.Utils
{
    public static class JsonResponse
    {

        public static string GetJsonResponse(AzureModel azureExportModel)
        {
            string url = azureExportModel.URL;
            string responseFromServer = string.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";
                request.Method = azureExportModel.RequestType;

                //Setting Headers if exist in request
                if (azureExportModel.WebHeaders != null && azureExportModel.WebHeaders.Count > 0)
                {

                    foreach (var item in azureExportModel.WebHeaders)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    }
                }

                if (azureExportModel.RequestType.Equals("POST"))
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = azureExportModel.JSON;
                        streamWriter.Write(json);
                    }
                }

                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.  
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.  
                        responseFromServer = reader.ReadToEnd();
                        // Display the content. 
                    }
                }
            }
            catch (WebException ex)
            {
                using (Stream dataStream = ex.Response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.  
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.  
                    responseFromServer = reader.ReadToEnd();
                    // Display the content. 
                }
            }

            return responseFromServer;
        }
    }
}
