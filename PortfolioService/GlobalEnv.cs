using System;
using System.Text.Json.Serialization;

namespace SALearning
{
    public class GlobalEnv
    {
        static GlobalEnv()
        {
            DBHOST = Environment.GetEnvironmentVariable("ENV_DBHOST"); // Database host name
            DBNAME = Environment.GetEnvironmentVariable("ENV_DBNAME"); // Database name
            DBUSER = Environment.GetEnvironmentVariable("ENV_DBUSER"); // Database user name
            DBPASSWORD = Environment.GetEnvironmentVariable("ENV_DBPASSWORD"); // Database password
            PRICEURL = Environment.GetEnvironmentVariable("ENV_PRICEURL"); // Host location of Pricing Service

            API_VERSION = "0.1.14";

            jsonOptions = new System.Text.Json.JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public static readonly string DBHOST;
        public static readonly string DBNAME;
        public static readonly string DBUSER;
        public static readonly string DBPASSWORD;
        public static readonly string PRICEURL;
        public static readonly string API_VERSION;

        public static readonly System.Text.Json.JsonSerializerOptions jsonOptions;

    }
}
