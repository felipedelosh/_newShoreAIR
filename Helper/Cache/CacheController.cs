using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Cache
{
    public class CacheController
    {
        public Dictionary<string, CacheItem> cacheMemory { get; set; }
        public string lastUrl { get; set; }

        public CacheController() { 
            cacheMemory = new Dictionary<string, CacheItem>();
        }

        public void AddCacheItem(string url, string data) {
            CacheItem temp = new CacheItem(url, data.ToString(), DateTime.Now);
            lastUrl = url;
            cacheMemory[url] = temp;
        }

        public bool isTheUrlDataInCache(string url) { 
            return cacheMemory.ContainsKey(url);
        }

        public string GetCacheByKey(string url) {
            string response = "";

            try {
                if (isTheUrlDataInCache(url))
                {
                    response = cacheMemory[url]._data;
                }
            }
            catch 
            {
                //Pass
            }
            
            return response;
        }

    }
}
