using Antique_shop;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary
{
   
        /// <summary>
        /// Единственный класс для работы с базой данных PostgreSQL.
        /// </summary>
        public class DBConnection : IUserRepository, IPurchaseRepository
        {
            private readonly string connectionString_;

            public DBConnection(string connectionString)
            {
            // Сохраняет строку подключения для следующих SQL-запросов.
                connectionString_ = connectionString;
            }

            public User FindByLogin(string login)
            {
            // Читает учётную запись и превращает результат запроса в User.
                using (var connection = new NpgsqlConnection(connectionString_))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"users\" WHERE \"login\" = @login";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string roleString = "Guest";
                                if (reader.FieldCount > 6 && !reader.IsDBNull(6))
                                {
                                    roleString = reader.GetString(6);
                                }

                                string email = "";
                                if (reader.FieldCount > 4 && !reader.IsDBNull(4))
                                {
                                    email = reader.GetString(4);
                                }

                                string phone = "";
                                if (reader.FieldCount > 5 && !reader.IsDBNull(5))
                                {
                                    phone = reader.GetString(5);
                                }

                                return new User
                                {
                                    Id = reader.GetInt32(0),
                                    Login = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    Email = email,
                                    Phone = phone,
                                    Role = User.ParseRole(roleString)
                                };
                            }
                        }
                    }
                }
                return null;
            }

            /// <summary>
            /// Проверяет, свободен ли логин.
            /// ИСПРАВЛЕНО: безопасное преобразование типа.
            /// </summary>
            public bool CheckIfLoginExists(string login)
            {
                // Проверяет, свободен ли логин: true означает, что пользователя нет.
                using (var connection = new NpgsqlConnection(connectionString_))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM \"users\" WHERE \"login\" = @login";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);

                        // БЕЗОПАСНОЕ ПОЛУЧЕНИЕ ЗНАЧЕНИЯ
                        object result = command.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            return true; // Логин свободен (защита от null)
                        }

                        // Convert.ToInt32 корректно преобразует Int64 в Int32
                        int count = Convert.ToInt32(result);

                        return count == 0;
                    }
                }
            }

            public bool AddUser(User user)
            {
                // Записывает новую учётную запись и её контактные данные.
                using (var connection = new NpgsqlConnection(connectionString_))
                {
                    connection.Open();

                    string query = "INSERT INTO \"users\" " +
                                   "(\"login\", \"password\", \"fullname\", email, phone, role) " +
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
                        return result > 0;
                    }
                }
            }

            public List<User> GetAllUsers()
            {
                // Загружает пользователей для административного списка.
                List<User> users = new List<User>();

                using (var connection = new NpgsqlConnection(connectionString_))
                {
                    connection.Open();
                    string query = "SELECT * FROM \"users\"";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string roleString = "Guest";
                                if (reader.FieldCount > 6 && !reader.IsDBNull(6))
                                {
                                    roleString = reader.GetString(6);
                                }

                                string email = "";
                                if (reader.FieldCount > 4 && !reader.IsDBNull(4))
                                {
                                    email = reader.GetString(4);
                                }

                                string phone = "";
                                if (reader.FieldCount > 5 && !reader.IsDBNull(5))
                                {
                                    phone = reader.GetString(5);
                                }

                                User user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    Login = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    FullName = reader.GetString(3),
                                    Email = email,
                                    Phone = phone,
                                    Role = User.ParseRole(roleString)
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
                return users;
            }

        public List<Tovar> GetAllTovars()
        {
            // Читает каталог вместе с названием категории для главной таблицы.
            List<Tovar> tovars = new List<Tovar>();

            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();

                string query = @"
                SELECT p.id, p.name, c.name AS category, p.era, p.price, 
                       p.author, p.country, p.description, p.authenticity_mark, p.photo
                FROM products p
                LEFT JOIN categories c ON p.category_id = c.id
                ORDER BY p.id";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Tovar tovar = new Tovar
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Name = reader["name"].ToString(),
                                Categoria = reader["category"].ToString(),
                                Era = reader["era"].ToString(),
                                Prise = Convert.ToDouble(reader["price"]),
                                Autor = reader["author"].ToString(),
                                Country = reader["country"].ToString(),
                                Opisanie = reader["description"].ToString(),
                                AuthenMark = Convert.ToBoolean(reader["authenticity_mark"]) ? "Да" : "Нет",
                                Foto = reader["photo"].ToString()
                            };
                            tovars.Add(tovar);
                        }
                    }
                }
            }
            return tovars;
        }

        // Удаляет купленные товары одной транзакцией, чтобы не оставить частичную покупку.
        public bool DeleteProducts(IEnumerable<int> productIds)
        {
            var ids = productIds.Distinct().ToList();
            if (ids.Count == 0) return false;

            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (int id in ids)
                        {
                            using (var command = new NpgsqlCommand("DELETE FROM products WHERE id=@id", connection, transaction))
                            {
                                command.Parameters.AddWithValue("id", id);
                                if (command.ExecuteNonQuery() != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<PurchaseRecord> GetPurchasesForUser(int userId)
        {
            const string sql = @"SELECT id, buyer_id, buyer_login, buyer_name, product_id, name, category, era, price,
                author, country, description, authenticity_mark, photo, quantity, purchased_at
                FROM purchases WHERE buyer_id=@userId ORDER BY purchased_at DESC, id DESC";
            return GetPurchaseRecords(sql, command => command.Parameters.AddWithValue("userId", userId));
        }

        public List<PurchaseRecord> GetPurchaseArchive()
        {
            const string sql = @"SELECT id, buyer_id, buyer_login, buyer_name, product_id, name, category, era, price,
                author, country, description, authenticity_mark, photo, quantity, purchased_at
                FROM purchases ORDER BY purchased_at DESC, id DESC";
            return GetPurchaseRecords(sql, command => { });
        }

        private List<PurchaseRecord> GetPurchaseRecords(string sql, Action<NpgsqlCommand> addParameters)
        {
            var records = new List<PurchaseRecord>();
            using (var connection = new NpgsqlConnection(connectionString_))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                addParameters(command);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(new PurchaseRecord
                        {
                            BuyerId = Convert.ToInt32(reader["buyer_id"]),
                            BuyerLogin = reader["buyer_login"].ToString(),
                            BuyerName = reader["buyer_name"].ToString(),
                            Quantity = Convert.ToInt32(reader["quantity"]),
                            PurchasedAt = Convert.ToDateTime(reader["purchased_at"]),
                            Product = new Tovar
                            {
                                Id = Convert.ToInt32(reader["product_id"]),
                                Name = reader["name"].ToString(),
                                Categoria = reader["category"].ToString(),
                                Era = reader["era"].ToString(),
                                Prise = Convert.ToDouble(reader["price"]),
                                Autor = reader["author"].ToString(),
                                Country = reader["country"].ToString(),
                                Opisanie = reader["description"].ToString(),
                                AuthenMark = Convert.ToBoolean(reader["authenticity_mark"]) ? "Да" : "Нет",
                                Foto = reader["photo"].ToString()
                            }
                        });
                    }
                }
            }
            return records;
        }

        public bool CompletePurchase(int userId, string buyerLogin, string buyerName, IEnumerable<CartItem> items)
        {
            var cartItems = items?.Where(item => item?.Product != null && item.Product.Id > 0 && item.Quantity > 0).ToList();
            if (userId <= 0 || cartItems == null || cartItems.Count == 0) return false;

            using (var connection = new NpgsqlConnection(connectionString_))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (CartItem item in cartItems)
                        {
                            const string insertSql = @"INSERT INTO purchases
                                (buyer_id, buyer_login, buyer_name, product_id, name, category, era, price, author, country,
                                 description, authenticity_mark, photo, quantity)
                                SELECT @buyerId, @buyerLogin, @buyerName, p.id, p.name, COALESCE(c.name, ''), p.era, p.price,
                                       p.author, p.country, p.description, p.authenticity_mark, p.photo, @quantity
                                FROM products p LEFT JOIN categories c ON c.id=p.category_id WHERE p.id=@productId";
                            using (var insert = new NpgsqlCommand(insertSql, connection, transaction))
                            {
                                insert.Parameters.AddWithValue("buyerId", userId);
                                insert.Parameters.AddWithValue("buyerLogin", buyerLogin ?? string.Empty);
                                insert.Parameters.AddWithValue("buyerName", buyerName ?? string.Empty);
                                insert.Parameters.AddWithValue("productId", item.Product.Id);
                                insert.Parameters.AddWithValue("quantity", item.Quantity);
                                if (insert.ExecuteNonQuery() != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }

                            using (var delete = new NpgsqlCommand("DELETE FROM products WHERE id=@productId", connection, transaction))
                            {
                                delete.Parameters.AddWithValue("productId", item.Product.Id);
                                if (delete.ExecuteNonQuery() != 1)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                            }
                        }
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Восстанавливает корзину пользователя из сохранённых строк БД.
        public List<CartItem> GetCartItems(int userId)
        {
            var items = new List<CartItem>();
            const string sql = @"SELECT p.id, p.name, c.name AS category, p.era, p.price,
                p.author, p.country, p.description, p.authenticity_mark, p.photo, ci.quantity
                FROM cart_items ci
                JOIN products p ON p.id = ci.product_id
                LEFT JOIN categories c ON c.id = p.category_id
                WHERE ci.user_id = @userId ORDER BY p.id";
            using (var connection = new NpgsqlConnection(connectionString_))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("userId", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Tovar
                        {
                            Id = Convert.ToInt32(reader["id"]), Name = reader["name"].ToString(),
                            Categoria = reader["category"].ToString(), Era = reader["era"].ToString(),
                            Prise = Convert.ToDouble(reader["price"]), Autor = reader["author"].ToString(),
                            Country = reader["country"].ToString(), Opisanie = reader["description"].ToString(),
                            AuthenMark = Convert.ToBoolean(reader["authenticity_mark"]) ? "Да" : "Нет",
                            Foto = reader["photo"].ToString()
                        };
                        items.Add(new CartItem(product, Convert.ToInt32(reader["quantity"])));
                    }
                }
            }
            return items;
        }

        // Добавляет товар в сохранённую корзину или увеличивает его количество.
        public bool AddCartItem(int userId, int productId)
        {
            const string sql = @"INSERT INTO cart_items (user_id, product_id, quantity) VALUES (@userId, @productId, 1)
                ON CONFLICT (user_id, product_id) DO UPDATE SET quantity = cart_items.quantity + 1";
            using (var connection = new NpgsqlConnection(connectionString_))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("productId", productId);
                connection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        // Удаляет одну позицию из корзины пользователя.
        public bool RemoveCartItem(int userId, int productId)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            using (var command = new NpgsqlCommand("DELETE FROM cart_items WHERE user_id=@userId AND product_id=@productId", connection))
            {
                command.Parameters.AddWithValue("userId", userId);
                command.Parameters.AddWithValue("productId", productId);
                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        // Удаляет все позиции корзины пользователя.
        public void ClearCartItems(int userId)
        {
            using (var connection = new NpgsqlConnection(connectionString_))
            using (var command = new NpgsqlCommand("DELETE FROM cart_items WHERE user_id=@userId", connection))
            {
                command.Parameters.AddWithValue("userId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}

