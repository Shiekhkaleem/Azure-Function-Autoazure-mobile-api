using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.API.ExceptionHandling
{
    
        internal static class FunctionContextExtensions
        {
            public static HttpRequestData GetHttpRequestData(this FunctionContext functionContext)
            {
                try
                {
                    KeyValuePair<Type, object> keyValuePair = functionContext.Features.SingleOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
                    if (keyValuePair.Equals(default(KeyValuePair<Type, object>))) return null;
                    object functionBindingsFeature = keyValuePair.Value;
                    if (functionBindingsFeature == null) return null;
                    Type type = functionBindingsFeature.GetType();
                    var inputData = type.GetProperties().Single(p => p.Name == "InputData").GetValue(functionBindingsFeature) as IReadOnlyDictionary<string, object>;
                    return inputData?.Values.SingleOrDefault(o => o is HttpRequestData) as HttpRequestData;
                }
                catch
                {
                    return null;
                }
            }
            public static async Task<HttpResponseData> GetHttpResponseData(this FunctionContext functionContext)
            {
                try
                {
                    var request =await  functionContext.GetHttpRequestDataAsync();
                    if (request == null) return null;
                    var response = HttpResponseData.CreateResponse(request);
                    var keyValuePair = functionContext.Features.FirstOrDefault(f => f.Key.Name == "IFunctionBindingsFeature");
                    if (keyValuePair.Equals(default(KeyValuePair<Type, object>))) return null;
                    object functionBindingsFeature = keyValuePair.Value;
                    if (functionBindingsFeature == null) return null;
                    PropertyInfo pinfo = functionBindingsFeature.GetType().GetProperty("InvocationResult");
                    pinfo.SetValue(functionBindingsFeature, response);
                    return response;
                }
                catch
                {
                    return null;
                }
            }
        }
}
