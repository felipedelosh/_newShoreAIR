using Helper.Cache;
using Microsoft.Extensions.Logging;
using Models.Contracts;
using RestSharp;
using System;

namespace Helper
{
    public class GetAPIData : IGetAPIData
    {
        private readonly ILogger<GetAPIData> _logger;
        private CacheController cache = new CacheController();
       

        public GetAPIData(ILogger<GetAPIData> logger) {
            _logger = logger;
        }

        public string GetHTTPServiceVr0(string url)
        {
            return GetHTTPServiceVrX(url, "0");
        }

        public string GetHTTPServiceVr1(string url)
        {
            return GetHTTPServiceVrX(url, "1");
        }


        public string GetHTTPServiceVr2(string url)
        {
            return GetHTTPServiceVrX(url, "2");
        }

        public string GetLastAPIUrl() {
            return cache.lastUrl;
        }

        public string GetCacheDataByUrl(string url) {
            return cache.GetCacheByKey(url);
        }

        /// <summary>
        /// Enter a ID of GET api response and try to return Json
        /// </summary>
        /// <param name="_v"></param>
        /// <returns></returns>
        public string GetHTTPServiceVrX(string url, string v)
        {
            string response = string.Empty;
            try
            {
                string _url = $"{url}/{v}";
                if (cache.isTheUrlDataInCache(_url)) {
                    response = cache.GetCacheByKey(_url);
                }
                else {
                    RestClient client = new RestClient(_url);
                    RestRequest request = new RestRequest();
                    request.Method = Method.Get;
                    request.AddHeader("Content-Type", "application/json");
                    RestResponse result = client.Execute(request);

                    if (result.IsSuccessStatusCode)
                    {
                        if (result.Content != null)
                        {
                            response = result.Content;
                            cache.AddCacheItem(_url, response);
                        }
                    }
                }


                _logger.LogInformation("The user get External Api information (newshore Flights)");
            }
            catch (Exception ex)
            {
               _logger.LogError($"Error to get data in external API (newshore Flights) {ex}");
                response = $"API_GET_ERR: {ex}";
            }

            return response;
        }

        public string GetDicHTTPServiceCurriences(string url)
        {

            string dataCurriences = "";

            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Content-Type", "application/json");
                RestResponse result = client.Execute(request);

                if (result.IsSuccessStatusCode)
                {
                    if (result.Content != null)
                    {
                        dataCurriences = result.Content;
                    }
                }
                else {
                    _logger.LogInformation("The user get External Api information (Curriencies)");
                }


            }
            catch (Exception ex) {
                _logger.LogError($"Error to get data in external API (Curriencies) {ex}");
            }

            return dataCurriences;
        }

    }
}
