using System;
using System.Data;
using Npgsql;

namespace DataBaseModule3Task
{
    class Program
    {
        static void Main(string[] args)
        {
            bool boolfound=false;
            using(NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; User Id=root; Password=root; Database=e-comerce"))
            {
                connection.Open();
                Console.WriteLine("connection established");
                
                while (true)
                {
                    Console.WriteLine("Choose an option");
                    Console.WriteLine("[1] AddProduct");
                    Console.WriteLine("[2] InscreaseStock");
                    Console.WriteLine("[3] ListProducts");
                    Console.WriteLine("[4] ListProductsWithCat");
                    Console.WriteLine("[5] ListEmptyCategories");
                    Console.WriteLine("[6] ListUsersWithUnpaidOrders");
                    Console.WriteLine("[7] ListFakeReviews");
                    Console.WriteLine("[8] ListSuppliersWhoBoughtTheirProducts");
                    Console.WriteLine("[9] ListCategoriesByBrandId");
                    Console.WriteLine("[10] exit");
                    Console.WriteLine("\r\nSelect an option: ");
                    
                    
                    switch (Console.ReadLine())
                    {
                        case "1": 
                            AddProduct(connection); 
                            break;
                        case "2": 
                            InscreaseStock(connection); 
                            break;
                        case "3": 
                            ListProducts(connection); 
                            break;
                        case "4": 
                            ListProductsWithCat(connection); 
                            break;
                        case "5": 
                            ListEmptyCategories(connection); 
                            break;
                        case "6": 
                            ListUsersWithUnpaidOrders(connection); 
                            break;
                        case "7": 
                            ListFakeReviews(connection); 
                            break;
                        case "8": 
                            ListSuppliersWhoBoughtTheirProducts(connection); 
                            break;
                        case "9":
                            ListCategoriesByBrandId(connection);
                            break;
                        case "10":
                            Console.WriteLine("GoodBye!");
                            break;
                    }
                    Console.WriteLine(String.Empty);
                    connection.Close();   
                }
            }
            
        }

        public static void AddProduct(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO products (name, supplier_id, brand_id, image, quantity, description, sub_sub_category_id, price) VALUES (@name, @supplier_id, @brand_id, @image, @quantity, @description, @sub_sub_category_id, @price)", connection);
                Console.WriteLine("Enter product name");
                cmd.Parameters.AddWithValue("@name", Console.ReadLine());
                Console.WriteLine("Enter supplier_id");
                cmd.Parameters.AddWithValue("@supplier_id", Int32.Parse(Console.ReadLine()));
                Console.WriteLine("Enter brand_id");
                cmd.Parameters.AddWithValue("@brand_id", Int32.Parse(Console.ReadLine()));
                Console.WriteLine("Enter product image");
                cmd.Parameters.AddWithValue("@image", Console.ReadLine());
                Console.WriteLine("Enter product quantity");
                cmd.Parameters.AddWithValue("@quantity", Int32.Parse(Console.ReadLine()));
                Console.WriteLine("Enter product description");
                cmd.Parameters.AddWithValue("@description", Console.ReadLine());
                Console.WriteLine("Enter product sub_sub_category_id");
                cmd.Parameters.AddWithValue("@sub_sub_category_id", Int32.Parse(Console.ReadLine()));
                Console.WriteLine("Enter product price");
                cmd.Parameters.AddWithValue("@price", Int32.Parse(Console.ReadLine()));
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Product added!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }

        public static void InscreaseStock(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE products SET quantity=quantity+@quantity  WHERE id=@id", connection);
                Console.WriteLine("Enter the product id");
                cmd.Parameters.AddWithValue("@id",  Int32.Parse(Console.ReadLine()));
                Console.WriteLine("Enter the product quantity");
                cmd.Parameters.AddWithValue("@quantity", Int32.Parse(Console.ReadLine()));
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Product Updated!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
        
        public static void ListProducts(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT * from products", connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("id={0}, name={1}, supplier_id={2}, brand_id={3}, image={4}, quantity={5}, description={6}, price={7}, sub_sub_category_id={8}", reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("supplier_id"), reader.GetInt32("brand_id"), reader.GetString("image"), reader.GetInt32("quantity"), reader.GetString("description"), reader.GetInt32("price"), reader.GetInt32("sub_sub_category_id"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }

        public static void ListUsersWithUnpaidOrders(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT users.id, users.name, users.email, orders.total_cost as order_total_cost FROM orders  Join users ON orders.customer_id = users.id  WHERE NOT EXISTS (SELECT null FROM payments WHERE payments.order_id = orders.id)", connection);
                var reader = cmd.ExecuteReader();
                
                Console.WriteLine();
                while (reader.Read())
                {
                    Console.WriteLine("id={0}, name={1}, email={2}", reader.GetInt32("id"), reader.GetString("name"), reader.GetString("email"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
        public static void ListProductsWithCat(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT products.id, products.name, products.quantity, products.supplier_id, products.brand_id, sub_sub_categories.name as sub_sub_cat_name, sub_categories.name as sub_cat_name, categories.name as cat_name from products JOIN sub_sub_categories ON products.sub_sub_category_id = sub_sub_categories.id  JOIN sub_categories ON sub_sub_categories.sub_category_id = sub_categories.id  JOIN categories ON sub_categories.category_id = categories.id", connection);
                var reader = cmd.ExecuteReader();
                
               while (reader.Read())
                {
                    Console.WriteLine("id={0}, name={1}, supplier_id={2}, brand_id={3}, image={4}, quantity={5}, description={6}, price={7}, sub_sub_category_id={8}", reader.GetInt32("id"), reader.GetString("name"), reader.GetInt32("supplier_id"), reader.GetInt32("brand_id"), reader.GetString("image"), reader.GetInt32("quantity"), reader.GetString("description"), reader.GetInt32("price"), reader.GetInt32("sub_sub_category_id"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
        public static void ListEmptyCategories(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT ssc.id, ssc.name FROM sub_sub_categories ssc LEFT JOIN products p ON p.sub_sub_category_id = ssc.id GROUP BY ssc.id, ssc.name HAVING COUNT (p.sub_sub_category_id)=0 UNION ALL SELECT sc.id, sc.name FROM sub_categories sc LEFT JOIN sub_sub_categories ssc ON ssc.sub_category_id = sc.id LEFT JOIN products p ON p.sub_sub_category_id = ssc.id GROUP BY sc.id, sc.name HAVING COUNT (p.sub_sub_category_id)=0 UNION ALL SELECT c.id, c.name FROM categories c  LEFT JOIN sub_categories sc ON sc.category_id = c.id LEFT JOIN sub_sub_categories ssc ON ssc.sub_category_id = sc.id LEFT JOIN products p ON p.sub_sub_category_id = ssc.id GROUP BY c.id, c.name HAVING COUNT (p.sub_sub_category_id)=0", connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("sub_sub_id={0}, sub_sub_name={1}, sub_cat_id={2}, sub_cat_name={3}, cat_id={4}, cat_name={5}", reader.GetInt32("sub_sub_cat_id"), reader.GetString("sub_sub_cat_name"), reader.GetInt32("sub_cat_id"), reader.GetString("sub_cat_name"), reader.GetInt32("cat_id"), reader.GetString("cat_name"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
        public static void ListCategoriesByBrandId(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT DISTINCT categories.id, categories.name FROM products JOIN sub_sub_categories ON products.sub_sub_category_id = sub_sub_categories.id JOIN sub_categories ON sub_sub_categories.sub_category_id = sub_categories.id  JOIN categories ON sub_categories.category_id = categories.id WHERE brand_id=@brand_id", connection);
                Console.WriteLine("Enter the brand id");
                cmd.Parameters.AddWithValue("@brand_id",  Int32.Parse(Console.ReadLine()));
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("id={0}, name={1}", reader.GetInt32("id"), reader.GetString("name"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
        public static void ListFakeReviews(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT  users.name As user_name, products.name AS product_name, reviews.id AS id, reviews.description AS description FROM reviews JOIN users ON users.id = reviews.customer_id JOIN products ON products.id = reviews.product_id JOIN orders ON orders.customer_id = reviews.customer_id JOIN carts ON carts.product_id != products.id  WHERE NOT EXISTS (SELECT null from orders o where o.cart_id != carts.id)", connection);
                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Console.WriteLine("It worked!!");
                    Console.WriteLine("user_name={0}, product_name={1}, id={2}, description={3}", reader.GetString("user_name"), reader.GetString("product_name"), reader.GetInt32("id"), reader.GetString("description"));
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
        public static void ListSuppliersWhoBoughtTheirProducts(NpgsqlConnection connection)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT DISTINCT users.id AS id, users.name AS name, users.email AS email FROM users JOIN orders ON orders.customer_id = users.id JOIN products ON products.supplier_id = orders.customer_id WHERE orders.is_paid = true", connection);
                var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    Console.WriteLine("It worked!!");
                    Console.WriteLine("id={0}, name={1}, email={2}", reader.GetInt64("id"), reader.GetString("name"), reader.GetString("email"));
                }
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            
        }
        
    }
}
