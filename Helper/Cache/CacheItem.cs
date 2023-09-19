using System;

namespace Helper.Cache
{
    public class CacheItem
    {

        public string _url { get; set; }
        public string _data { get; set; }
        public DateTime _date { get; set; }

        private int MinToAbsoluteExpiration { get; set; }


        public CacheItem(string url, string data, DateTime date)
        {
            _url = url;
            _data = data;
            _date = date;
            MinToAbsoluteExpiration = 30;
        }


    }
}
