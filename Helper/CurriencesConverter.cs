using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Helper
{
    /// <summary>
    /// https://docs.openexchangerates.org/reference/api-introduction 
    /// This is a Currencies converter. the base price is USD 
    /// use GetInConvertion(ISO_CODE, $) to calculate a convertion
    /// </summary>
    public class CurriencesConverter
    {
        private Dictionary<string, double> _currencies;

        public CurriencesConverter(string data) {
            _currencies = new Dictionary<string, double>();
            //Default currience by api
            
            try {
                //Update 
                Dictionary<string, object> jsonCurriencesData = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
                _currencies[jsonCurriencesData["base"].ToString()] = Convert.ToDouble(1); ; //USD = 1
                Dictionary<string, object> rates = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonCurriencesData["rates"].ToString());

                foreach (var i in rates)
                {
                    string key = i.Key;
                    double value = Convert.ToDouble(i.Value);
                   _currencies[key] = value;
                }
                
                //Loger exito en curriences
            }
            catch (Exception ex)
            {
                //Logger error en curriences
            }

        }

        /// <summary>
        /// try Return convertion value or -999999 if fail. 
        /// </summary>
        /// <param name="isoCurrience"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public Double GetInConvertion(string isoCurrience, double qty) {
            try {
                return qty * _currencies[isoCurrience];
            }
            catch {
                return -9999999;
            }
        }

    }
}
