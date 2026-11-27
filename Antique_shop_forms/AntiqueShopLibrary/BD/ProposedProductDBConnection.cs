using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShopLibrary.BD
{
    public interface IProposedProductRepository : ICustomerRequestRepository
    {
        // Контракт доступа к предложениям: форма и сервис работают через эти методы.
        bool AddProposedProduct(ProposedProduct proposedProduct);
        List<ProposedProduct> GetProposedProducts();
        ProposedProduct GetProposedProductById(int id);
        bool UpdateProposedProduct(ProposedProduct proposedProduct);
        bool DeleteProposedProduct(int id);
        bool ApproveProduct(int id);
        bool RejectProduct(int id);
        bool MarkForReview(int id);
    }

    public class ProposedProductDBConnection : IProposedProductRepository
    {
        // Строка подключения передаётся из приложения.
        private readonly string _connectionString;

            public ProposedProductDBConnection(string connectionString)
            {
                _connectionString = connectionString;
            }

            public bool AddProposedProduct(ProposedProduct product)
            {
                // Сохраняет заполненную форму в requests как заявку на рассмотрение.
                try
                {
                    using (var conn = new NpgsqlConnection(_connectionString))
                    {
                        conn.Open();
                        string query = @"
                        INSERT INTO requests (name, category, era, price, author, country, description, phone, photo, submitted_by_user_id) 
                        VALUES (@name, @category, @era, @price, @author, @country, @description, @phone, @photo, @submitted_by_user_id)";

                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("name", product.Name);
                            cmd.Parameters.AddWithValue("category", product.Category);
                            cmd.Parameters.AddWithValue("era", product.Era);
                            cmd.Parameters.AddWithValue("price", product.Price);
                            cmd.Parameters.AddWithValue("author", product.Author);
                            cmd.Parameters.AddWithValue("country", product.Country);
                            cmd.Parameters.AddWithValue("description", product.Description);
                            cmd.Parameters.AddWithValue("phone", product.Phone);
                            cmd.Parameters.AddWithValue("photo", product.PhotoPath ?? "");
                            cmd.Parameters.AddWithValue("submitted_by_user_id", product.SubmittedByUserId.HasValue ? (object)product.SubmittedByUserId.Value : DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public List<ProposedProduct> GetProposedProducts()
            {
                // Загружает очередь заявок и переводит каждую строку БД в модель.
                var list = new List<ProposedProduct>();
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("SELECT id, name, category, era, price, author, country, description, phone, photo, status, submitted_by_user_id, catalog_product_id FROM requests WHERE status IN ('pending', 'review') ORDER BY id", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) list.Add(ReadProduct(reader));
                    }
                }
                return list;
            }

            public ProposedProduct GetProposedProductById(int id)
            {
                // Ищет одну заявку по её идентификатору.
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("SELECT id, name, category, era, price, author, country, description, phone, photo, status, submitted_by_user_id, catalog_product_id FROM requests WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader()) return reader.Read() ? ReadProduct(reader) : null;
                }
            }

            public bool UpdateProposedProduct(ProposedProduct product)
            {
                // Обновляет поля заявки; true означает, что строка найдена и изменена.
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("UPDATE requests SET name=@name, category=@category, era=@era, price=@price, author=@author, country=@country, description=@description, phone=@phone, photo=@photo WHERE id=@id", conn))
                {
                    AddProductParameters(cmd, product);
                    cmd.Parameters.AddWithValue("id", product.Id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() == 1;
                }
            }

            public bool DeleteProposedProduct(int id)
            {
                // Удаляет заявку по id; используется при отклонении.
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("DELETE FROM requests WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() == 1;
                }
            }

            public List<ProposedProduct> GetRequestsForUser(int userId)
            {
                var list = new List<ProposedProduct>();
                const string sql = @"SELECT id, name, category, era, price, author, country, description, phone, photo,
                    status, submitted_by_user_id, catalog_product_id
                    FROM requests WHERE submitted_by_user_id=@userId ORDER BY id DESC";
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("userId", userId);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read()) list.Add(ReadProduct(reader));
                }
                return list;
            }

            public bool DeleteRequestForUser(int requestId, int userId)
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("DELETE FROM requests WHERE id=@id AND submitted_by_user_id=@userId", conn))
                {
                    cmd.Parameters.AddWithValue("id", requestId);
                    cmd.Parameters.AddWithValue("userId", userId);
                    conn.Open();
                    return cmd.ExecuteNonQuery() == 1;
                }
            }

            public bool RejectProduct(int id)
            {
                // Сохраняет результат модерации для личной истории клиента и убирает заявку из очереди.
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("UPDATE requests SET status='rejected' WHERE id=@id AND status IN ('pending', 'review')", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() == 1;
                }
            }

            public bool MarkForReview(int id)
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                using (var cmd = new NpgsqlCommand("UPDATE requests SET status='review' WHERE id=@id AND status IN ('pending', 'review')", conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    conn.Open();
                    return cmd.ExecuteNonQuery() == 1;
                }
            }

            public bool ApproveProduct(int id)
            {
                // Переносит заявку в каталог и удаляет её из очереди одной транзакцией.
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Блокировка строки защищает заявку от параллельного одобрения.
                            ProposedProduct product;
                            using (var select = new NpgsqlCommand("SELECT id, name, category, era, price, author, country, description, phone, photo, status, submitted_by_user_id, catalog_product_id FROM requests WHERE id=@id AND status IN ('pending', 'review') FOR UPDATE", conn, transaction))
                            {
                                select.Parameters.AddWithValue("id", id);
                                using (var reader = select.ExecuteReader())
                                    product = reader.Read() ? ReadProduct(reader) : null;
                            }
                            if (product == null) { transaction.Rollback(); return false; }

                            // Находит категорию или создаёт её, если такой ещё нет.
                            object categoryId = DBNull.Value;
                            if (!string.IsNullOrWhiteSpace(product.Category))
                            {
                                using (var category = new NpgsqlCommand("SELECT id FROM categories WHERE name=@name", conn, transaction))
                                {
                                    category.Parameters.AddWithValue("name", product.Category);
                                    categoryId = category.ExecuteScalar() ?? DBNull.Value;
                                }
                                if (categoryId == DBNull.Value)
                                {
                                    using (var category = new NpgsqlCommand("INSERT INTO categories (name) VALUES (@name) RETURNING id", conn, transaction))
                                    {
                                        category.Parameters.AddWithValue("name", product.Category);
                                        categoryId = category.ExecuteScalar();
                                    }
                                }
                            }

                            // Новый товар сначала считается непроверенным по подлинности.
                            const string insertSql = @"INSERT INTO products
                                (name, category_id, era, price, author, country, description, authenticity_mark, photo)
                                VALUES (@name, @category, @era, @price, @author, @country, @description, FALSE, @photo)
                                RETURNING id";
                            object newProductId;
                            using (var insert = new NpgsqlCommand(insertSql, conn, transaction))
                            {
                                insert.Parameters.AddWithValue("name", product.Name ?? "");
                                insert.Parameters.AddWithValue("category", categoryId);
                                insert.Parameters.AddWithValue("era", product.Era ?? "");
                                insert.Parameters.AddWithValue("price", product.Price);
                                insert.Parameters.AddWithValue("author", product.Author ?? "");
                                insert.Parameters.AddWithValue("country", product.Country ?? "");
                                insert.Parameters.AddWithValue("description", product.Description ?? "");
                                insert.Parameters.AddWithValue("photo", product.PhotoPath ?? "");
                                newProductId = insert.ExecuteScalar();
                                if (newProductId == null || newProductId == DBNull.Value) { transaction.Rollback(); return false; }
                            }
                            // Сохраняет статус для клиента; активная очередь его больше не показывает.
                            using (var update = new NpgsqlCommand("UPDATE requests SET status='approved', catalog_product_id=@productId WHERE id=@id", conn, transaction))
                            {
                                update.Parameters.AddWithValue("productId", Convert.ToInt32(newProductId));
                                update.Parameters.AddWithValue("id", id);
                                if (update.ExecuteNonQuery() != 1) { transaction.Rollback(); return false; }
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

            private static void AddProductParameters(NpgsqlCommand cmd, ProposedProduct product)
            {
                // Передаёт значения отдельно от SQL-текста, чтобы корректно обработать ввод.
                cmd.Parameters.AddWithValue("name", product.Name ?? "");
                cmd.Parameters.AddWithValue("category", product.Category ?? "");
                cmd.Parameters.AddWithValue("era", product.Era ?? "");
                cmd.Parameters.AddWithValue("price", product.Price);
                cmd.Parameters.AddWithValue("author", product.Author ?? "");
                cmd.Parameters.AddWithValue("country", product.Country ?? "");
                cmd.Parameters.AddWithValue("description", product.Description ?? "");
                cmd.Parameters.AddWithValue("phone", product.Phone ?? product.AdditionalInfo ?? "");
                cmd.Parameters.AddWithValue("photo", product.PhotoPath ?? "");
            }

            private static ProposedProduct ReadProduct(NpgsqlDataReader reader)
            {
                // Сопоставляет столбцы SELECT с полями модели ProposedProduct.
                return new ProposedProduct
                {
                    Id = reader.GetInt32(0), Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Category = reader.IsDBNull(2) ? "" : reader.GetString(2), Era = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Price = reader.IsDBNull(4) ? 0 : Convert.ToDouble(reader.GetValue(4)), Author = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Country = reader.IsDBNull(6) ? "" : reader.GetString(6), Description = reader.IsDBNull(7) ? "" : reader.GetString(7),
                    Phone = reader.IsDBNull(8) ? "" : reader.GetString(8), AdditionalInfo = reader.IsDBNull(8) ? "" : reader.GetString(8),
                    PhotoPath = reader.IsDBNull(9) ? "" : reader.GetString(9),
                    Status = reader.IsDBNull(10) ? "pending" : reader.GetString(10),
                    SubmittedByUserId = reader.IsDBNull(11) ? (int?)null : Convert.ToInt32(reader.GetValue(11)),
                    CatalogProductId = reader.IsDBNull(12) ? (int?)null : Convert.ToInt32(reader.GetValue(12))
                };
            }
        
    }
}
