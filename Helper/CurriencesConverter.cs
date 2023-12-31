﻿using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Models.Contracts;

namespace Helper
{
    /// <summary>
    /// https://docs.openexchangerates.org/reference/api-introduction 
    /// This is a Currencies converter. the base price is USD 
    /// use GetInConvertion(ISO_CODE, $) to calculate a convertion
    /// </summary>
    public class CurriencesConverter : ICurriencesConverter
    {
        private readonly ILogger<CurriencesConverter> _logger;
        private Dictionary<string, double> _currencies;

        public CurriencesConverter(ILogger<CurriencesConverter> logger) {
            _logger = logger;
        }

        public void UpdateCurriences(string data) {
            _currencies = new Dictionary<string, double>();
            //Default currience by api

            try
            {
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

                _logger.LogInformation("The user get and update information of curriences");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error While the user GET and Update curriences {ex}");
            }

        }

        /// <summary>
        /// try Return convertion value or -999999 if fail. 
        /// </summary>
        /// <param name="isoCurrience"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public double GetInConvertion(string isoCurrience, double qty) {
            try {
                double convertQTY = qty * _currencies[isoCurrience];
                convertQTY = Math.Round(convertQTY, 2);
                return convertQTY;
            }
            catch {
                _logger.LogError($"The user inser a invalid currience {isoCurrience}");
                return -99999999;
            }
        }

    }
}
