using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using ShoesShop.Core.Models;
using static ShoesShop.Core.Services.DataAcess.IDao;

namespace ShoesShop.Core.Services.DataAcess;
public class PostgreDao : IDao
{
    private NpgsqlConnection dbConnection = null;
    public PostgreDao()
    {
        var connectionConfig = """
            Host = localhost;
            Port=5433;
            Database = supershop;
            User ID = root;
            Password = root;
        """;
        dbConnection = new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    public PostgreDao(string serverUrl, string databaseName, string userId, string password)
    {
        var connectionConfig = $"""
            Server = {serverUrl};
            Database = {databaseName};
            User Id = {userId};
            Password = {password};
            TrustServerCertificate = True;
        """;
        var dbConnection = new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    ~PostgreDao()
    {
        dbConnection.Close();
    }

    private string GetSortString(string[] sortFields, Dictionary<string, SortType> sortOptions)
    {
        var sortString = "ORDER BY ";
        var useDefaultSort = true;
        var countSortFields = 0;
        foreach (var item in sortOptions)
        {
            if (sortFields.Contains(item.Key))
            {
                useDefaultSort = false;
                if (countSortFields > 0)
                {
                    sortString += ", ";
                }
                if (item.Value == SortType.Ascending)
                {
                    sortString += $"{item.Key} asc";
                }
                else
                {
                    sortString += $"{item.Key} desc";
                }
                countSortFields++;
            }
        }
        if (useDefaultSort)
        {
            sortString += "\"CategoryID\"";
        }
        return sortString;
    }

    private string GetWhereString(string[] whereFields, Dictionary<string, string> whereOptions)
    {
        var whereString = """\nWHERE""";
        var useDefaultWhere = true;
        var countWhereFields = 0;
        foreach (var item in whereOptions)
        {
            if (whereFields.Contains(item.Key))
            {
                useDefaultWhere = false;
                if (countWhereFields > 0)
                {
                    whereString += " AND ";
                }
                whereString += $"\"{item.Key}\" LIKE \"{item.Value}\"";
                countWhereFields++;
            }
        }
        if (useDefaultWhere)
        {
            whereString = "";
        }
        return whereString;
    }
    public Tuple<List<Category>, long> GetCategories(
        int page, int rowsPerPage,
        Dictionary<string, string> whereOptions,
        Dictionary<string, IDao.SortType> sortOptions)
    {
        var categoryFields = new string[]{
            "ID", "Name","Description"
        };
        var result = new List<Category>();
        var sortString = GetSortString(categoryFields, sortOptions);
        var whereString = GetWhereString(categoryFields, whereOptions);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, "ID","Name","Description"
            FROM "Category"{whereString}
            {sortString}
            LIMIT @Take 
            OFFSET @Skip
            """;
        var command = new NpgsqlCommand(sqlQuery, dbConnection);
        command.Parameters.Add("@Skip", NpgsqlDbType.Integer)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", NpgsqlDbType.Integer)
            .Value = rowsPerPage;
        var reader = command.ExecuteReader();
        long totalCategories = 0;
        while (reader.Read())
        {
            if (totalCategories == 0)
            {
                totalCategories = (long)reader["Total"];
            }
            var category = new Category();
            category.ID = (int)reader["ID"];
            category.Name = (string)reader["Name"];
            category.Description = (string)reader["Description"];
            result.Add(category);
        }
        return new Tuple<List<Category>, long>(
            result, totalCategories
        );
    }
    public Tuple<List<OrderDetail>, int> GetOrderDetailsByID(int orderID, int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
    public Tuple<List<Order>, int> GetOrders(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
    public Tuple<List<Shoes>, int> GetShoes(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
    public User GetUserByID(int userID) => throw new NotImplementedException();
    public Tuple<List<User>, int> GetUsers(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
}
