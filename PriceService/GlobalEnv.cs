using System;

namespace PriceService
{
    public class GlobalEnv
    {
        static GlobalEnv()
        {
            DBHOST = Environment.GetEnvironmentVariable("ENV_DBHOST"); // Database host name
            DBNAME = Environment.GetEnvironmentVariable("ENV_DBNAME"); // Database name
            DBUSER = Environment.GetEnvironmentVariable("ENV_DBUSER"); // Database user name
            DBPASSWORD = Environment.GetEnvironmentVariable("ENV_DBPASSWORD"); // Database password
            IEX_KEY = Environment.GetEnvironmentVariable("ENV_IEX_KEY"); // Key for Alpha Advantage Stock service
            API_VERSION = "1.0.5";
        }

        public static readonly string DBHOST;
        public static readonly string DBNAME;
        public static readonly string DBUSER;
        public static readonly string DBPASSWORD;
        public static readonly string IEX_KEY;
        public static readonly string API_VERSION;
    }
}
