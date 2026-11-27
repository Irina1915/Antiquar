using System;
using Npgsql;

namespace AntiqueShopLibrary
{
    public class DatabaseConfig
    {
        // Пароль хранится в переменной Windows, а не в исходном коде.
        public static string ConnectionString
        {
            get
            {
                string password = Environment.GetEnvironmentVariable("ANTIQUE_SHOP_DB_PASSWORD");
                if (string.IsNullOrWhiteSpace(password))
                    throw new InvalidOperationException("Не задана переменная среды ANTIQUE_SHOP_DB_PASSWORD.");

                return new NpgsqlConnectionStringBuilder
                {
                    // Новая локальная БД проекта, отдельный экземпляр PostgreSQL.
                    Host = "127.0.0.1", Port = 5433, Username = "antique_app",
                    Password = password, Database = "antique_shop"
                }.ConnectionString;
            }
        }

        // Прежнее активное подключение к локальному PostgreSQL (теперь отключено):
        // Host=localhost;Port=5432;Username=antique_app;Database=antique_shop
        // Старая колледжная БД сохранена для справки:
        // Host=192.168.1.48;Port=5432;Username=st53-7;Database=korona

    }
}
