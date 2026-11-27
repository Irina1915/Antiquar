using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Collections.Generic;

namespace Antique_shop_library
{
    /// <summary>
    /// Класс для работы с базой данных PostgreSQL.
    /// </summary>
    public class DBConnection : IUserRepository
    {
        private readonly string connectionString_;

        public DBConnection(string connectionString)
        {
            connectionString_ = connectionString;
        }

        /// <summary>
        /// Поиск пользователя по логину.
        /// </summary>
        public User FindByLogin(string login)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                string query = "SELECT * FROM \"Users\" WHERE \"логин\" = @login";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string roleString = reader.IsDBNull(6) ? "Guest" : reader.GetString(6);

                            return new User
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                Phone = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Role = User.ParseRole(roleString)
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Возвращает true, если логин СВОБОДЕН (не занят).
        /// </summary>
        public bool CheckIfLoginExists(string login)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM \"Users\" WHERE \"логин\" = @login";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);

                    int count = (int)command.ExecuteScalar();

                    if (count == 0)
                    {
                        return true; // Логин свободен
                    }
                    else
                    {
                        return false; // Логин занят
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет нового пользователя в базу данных.
        /// Пароль сохраняется в виде хэша (уже захэширован в User.SetPassword).
        /// </summary>
        public bool AddUser(User user)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                string query = "INSERT INTO \"Users\" " +
                               "(\"логин\", \"пароль\", \"Фамилия и имя\", email, phone, role) " +
                               "VALUES (@login, @password, @fullname, @email, @phone, @role)";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", user.Login);
                    command.Parameters.AddWithValue("@password", user.Password);
                    command.Parameters.AddWithValue("@fullname", user.FullName);
                    command.Parameters.AddWithValue("@email", user.Email ?? "");
                    command.Parameters.AddWithValue("@phone", user.Phone ?? "");
                    command.Parameters.AddWithValue("@role", User.RoleToString(user.Role));

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Получает список всех пользователей.
        /// </summary>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                string query = "SELECT * FROM \"Users\"";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string roleString = reader.IsDBNull(6) ? "Guest" : reader.GetString(6);

                            User user = new User
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Email = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                Phone = reader.IsDBNull(5) ? "" : reader.GetString(5),
                                Role = User.ParseRole(roleString)
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
    }
}