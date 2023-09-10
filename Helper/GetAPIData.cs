using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class GetAPIData
    {
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


        /// <summary>
        /// Enter a ID of GET api response and try to return Json
        /// </summary>
        /// <param name="_v"></param>
        /// <returns></returns>
        public string GetHTTPServiceVrX(string url, string v)
        {


            var response = string.Empty;
            try
            {
                var client = new RestClient(url + "/" + v);
                var request = new RestRequest();
                request.Method = Method.Get;
                request.AddHeader("Content-Type", "application/json");
                RestResponse result = client.Execute(request);

                if (result.IsSuccessStatusCode)
                {
                    if (result.Content != null)
                    {
                        response = result.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                response = "API_GET_ERR:" + ex;
            }

            return response;
        }

    }
}
