using System;

namespace FeudalMP.src.util
{
    public class CFG
    {
        public class Application
        {
            public const string SECTION = "Application";
            public const string FIRST_TIME_RUN = "first_time_run";
        }

        public class Server
        {
            public const string SECTION = "Server";
            public const string PORT = "port";
            public const string DATABASE_HOST = "database_host";
            public const string DATABASE_PORT = "database_port";
            public const string DATABASE_NAME = "database_name";
            public const string DATABASE_USER = "database_user";
            public const string DATABASE_USER_PASSWORD = "database_user_password";
        }
    }
}