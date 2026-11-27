using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary
{
    /// <summary>
    /// Класс для работы с таблицей Users в базе данных PostgreSQL.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString_;

        /// <summary>
        /// Конструктор класса. Принимает строку подключения к базе данных.
        /// </summary>
        public UserRepository(string connectionString)
        {
            connectionString_ = connectionString;
        }

        /// <summary>
        /// Поиск пользователя по логину.
        /// Формирует и выполняет SQL-запрос SELECT * FROM Users WHERE логин = @login.
        /// </summary>
        /// <param name="login">Логин пользователя для поиска.</param>
        /// <returns>Объект User с заполненными данными или null, если пользователь не найден.</returns>
        public User FindByLogin(string login)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                // Запрос строго по ТЗ: SELECT * FROM Users WHERE Login = @login
                string query = "SELECT * FROM \"Users\" WHERE \"логин\" = @login";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Порядок полей из БД: ID, логин, пароль, Фамилия и имя
                            string loginValue = reader.GetString(1);
                            string role = "Customer";

                            if (loginValue.ToLower() == "admin")
                            {
                                role = "Admin";
                            }

                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Role = User.ParseRole(reader.GetString(4)),
                                Email = "",
                                Phone = ""
                            };
                        }
                    }
                }
            }
            return null;
        }

        public bool CheckIfLoginExists(string login)
        {
            throw new NotImplementedException();
        }

        public bool AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
