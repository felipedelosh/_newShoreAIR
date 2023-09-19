using System;
using System.Collections.Generic;

namespace Helper
{
    /// <summary>
    /// This is a Currencies converter. the base price is USD 
    /// use GetInConvertion(ISO_CODE, $) to calculate a convertion
    /// </summary>
    public class CurriencesConverter
    {
        private Dictionary<string, double> currencies = new Dictionary<string, double>();

        public CurriencesConverter() {
            //Simulate GET API
            currencies["USD"] = 1;
            currencies["COP"] = 4016.83;
            currencies["MXN"] = 17.5;
            currencies["EUR"] = 0.93;
        }

        /// <summary>
        /// try Return convertion value or -999999 if fail. 
        /// </summary>
        /// <param name="isoCurrience"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public Double GetInConvertion(string isoCurrience, double qty) {
            try {
                return qty * currencies[isoCurrience];
            }
            catch {
                return -9999999;
            }
        }

    }
}
