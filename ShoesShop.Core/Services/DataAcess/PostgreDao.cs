using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ShoesShop;
public class PostgreDao : IDao
{
    private readonly string connectionString;
    public PostgreDao()
    {

        this.connectionString = 
        "Host=localhost;" +
        "Username=postgres;" +
        "Password=123;" +
        "Database=demoshoeshop"; ;
    }

    public Tuple<List<Shoes>, int> GetShoes(
               int page, int rowsPerPage,
                      string keyword,
                             Dictionary<string, IDao.SortType> sortOptions
           )
    {

        List<Shoes> shoes = new List<Shoes>();
        var totalRows = 0;
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {

            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand())
            {

                command.Connection = connection;
                //command.CommandText = "SELECT COUNT(*) FROM Shoes";
                //totalRows = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "SELECT * FROM Shoes";
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        Shoes shoe = new Shoes();
                        shoe.ID = reader.GetInt32(0);
                        shoe.Name = reader.GetString(1);
                        //shoe.Size = reader.GetString(2);
                        //shoe.Color = reader.GetString(2);
                        //shoe.Price = (float)reader.GetDecimal(2);
                        //shoe.Description = reader.GetString(3);
                        //shoes.Add(shoe);
                    }
                }
            }
        }
        return new Tuple<List<Shoes>, int>(shoes, totalRows);
    }

    public bool DeleteShoes(int id)
    {

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {

            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand())
            {

                command.Connection = connection;
                command.CommandText = "DELETE FROM shoes WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);
                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    public bool AddShoes(Shoes info)
    {

        //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        //{

        //    connection.Open();
        //    using (NpgsqlCommand command = new NpgsqlCommand())
        //    {

        //        command.Connection = connection;
        //        command.CommandText = "INSERT INTO shoes (name, price, description, image) VALUES (@name, @price, @description, @image)";
        //        command.Parameters.AddWithValue("@name", info.Name);
        //        command.Parameters.AddWithValue("@price", info.Price);
        //        command.Parameters.AddWithValue("@description", info.Description);
        //        command.Parameters.AddWithValue("@image", info.Image);
        //        return command.ExecuteNonQuery() > 0;
        //    }
        //}
        return true;
    }

    public bool UpdateShoes(Shoes info)
    {
        //using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        //{


        //    connection.Open();
        //    using (NpgsqlCommand command = new NpgsqlCommand())
        //    {


        //        command.Connection = connection;
        //        command.CommandText = "UPDATE shoes SET name = @name, price = @price, description = @description, image = @image WHERE id = @id";
        //        command.Parameters.AddWithValue("@name", info.Name);
        //        command.Parameters.AddWithValue("@price", info.Price);
        //        command.Parameters.AddWithValue("@description", info.Description);
        //        command.Parameters.AddWithValue("@image", info.Image);
        //        command.Parameters.AddWithValue("@id", info.Id);
        //        return command.ExecuteNonQuery() > 0;
        //    }
        //}
        return true;
    }
}
