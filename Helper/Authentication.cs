using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class Authentication
    {
        const string TOKEN = "kmzwa8awaa";


        /// <summary>
        /// Validates a Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool isTokenValid(string token)
        {

            const string TOKEN = "kmzwa8awaa";

            try
            {
                string _t = token.Split(' ')[1];
                return _t == TOKEN;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
    }
}
