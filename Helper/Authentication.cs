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
            //Simulate to get Database : get Token By User
            const string TOKEN = "kmzwa8awaa";

            try
            {
                string [] _t = new string[2];
                _t[0] = token.Split(' ')[0];
                _t[1] = token.Split(' ')[1];

                return (_t[0].ToLower() == "bearer" || _t[0].ToLower() == "apikey") && (_t[1] == TOKEN);
            }
            catch
            {
                return false;
            }

        }
    }
}
