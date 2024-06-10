using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace DAL.Api
{
    public class ApiService : IApi
    {
        private ApiService()
        {
        }

        public static ApiService Instance { get; } = new();

        public async Task<T> GetData<T>(string endpoint)
        {
            var apiClient = new RestClient(endpoint);
            var request = new RestRequest("");

                

            var apiResult = await apiClient.ExecuteAsync(request);

               

            if (apiResult.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    if (apiResult.Content != null)
                    {
                        return JsonConvert.DeserializeObject<T>(apiResult.Content) ??
                               throw new InvalidOperationException();
                    }
                }
                catch (JsonSerializationException jsonEx)
                {
                    throw new Exception($"JSON deserialization error: {jsonEx.Message}. JSON content: {apiResult.Content}", jsonEx);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unexpected error during JSON deserialization: {ex.Message}. JSON content: {apiResult.Content}", ex);
                }

                return default!;
            }

            throw new HttpRequestException($"HTTP request error: {apiResult.StatusDescription} (Status code: {apiResult.StatusCode}). Endpoint: {endpoint}");
        }

        
    }
}
