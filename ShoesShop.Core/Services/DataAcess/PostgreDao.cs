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

    public PostgreDao(string serverUrl,int port, string databaseName, string userId, string password)
    {
        var connectionConfig = $"""
            Server = {serverUrl};
            Port={port};
            Database = {databaseName};
            User Id = {userId};
            Password = {password};
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
                    sortString += $"\"{item.Key}\" asc";
                }
                else
                {
                    sortString += $"\"{item.Key}\" desc";
                }
                countSortFields++;
            }
        }
        if (useDefaultSort)
        {
            sortString = "";
        }
        return sortString;
    }

    private string GetWhereConditionNumberFields(string[] numberFields, Dictionary<string, Tuple<decimal, decimal>> numberOptions)
    {
        string result = "";
        var countNumberFields= 0;
        foreach (var item in numberOptions) {
            if (numberFields.Contains(item.Key)){
                if(countNumberFields > 0)
                {
                    result += " AND ";
                }
                result += $"\"{item.Key}\" BETWEEN {item.Value.Item1} AND {item.Value.Item2}";
                countNumberFields++;
            }
        }
        return result;
    }
    private string GetWhereConditionTextFields(string[] textFields, Dictionary<string, string> textOptions)
    {
        var whereString = "";
        var countWhereFields = 0;
        foreach (var item in textOptions)
        {
            if (textFields.Contains(item.Key))
            {
                if (countWhereFields > 0)
                {
                    whereString += " AND ";
                }
                whereString += $"\"{item.Key}\" LIKE \'%{item.Value}%\'";
                countWhereFields++;
            }
        }
        return whereString;
    }

    private string GetWhereCondition(string[] numberFields, Dictionary<string, Tuple<decimal, decimal>> numberOptions,
        string[] textFields, Dictionary<string, string> textOptions)
    {
        var res = "";
        var textFieldsCondition=GetWhereConditionTextFields(textFields, textOptions);
        var numberFieldsCondition=GetWhereConditionNumberFields(numberFields,numberOptions);
        if(numberFieldsCondition != "" && textFieldsCondition != "")
        {
            res +=$"WHERE {numberFieldsCondition} AND {textFieldsCondition}";
        }
        else if(numberFieldsCondition !="" || textFieldsCondition != "")
        {
            if(numberFieldsCondition != "")
            {
                res += $"WHERE {numberFieldsCondition}";
            }
            else
            {
                res += $"WHERE {textFieldsCondition}";
            }
        }
        return res;
    }

    private String GetSelectFields(string[] entityFields)
    {
        var selectFields = "";
        for (var i = 0; i < entityFields.Length; i++)
        {
            selectFields += $"\"{entityFields[i]}\"";
            if (i != entityFields.Length - 1)
            {
                selectFields += ",";
            }
        }
        return selectFields;
    }
    public Tuple<List<Category>, long> GetCategories(
        int page, int rowsPerPage,
        Dictionary<string,Tuple<decimal,decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions)
    {
        var categoryTextFields = new string[]{
            "Name","Description"
        };
        var categoryNumberFields = new string[]
        {
            "CategoryID",
        };
        var categoryFields = categoryTextFields.Concat(categoryNumberFields).ToArray();
        var result = new List<Category>();
        var sortString = GetSortString(categoryFields, sortOptions);
        var whereString = GetWhereCondition(categoryNumberFields,numberFieldsOptions,categoryTextFields,textFieldsOptions);
        //var selectFieldsString = GetSelectFields(categoryFields);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, "CategoryID","Name","Description"
            FROM "Category" {whereString}
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
            category.ID = (int)reader["CategoryID"];
            category.Name = (string)reader["Name"];
            category.Description = (string)reader["Description"];
            result.Add(category);
        }
        reader.Close();
        return new Tuple<List<Category>, long>(
            result, totalCategories
        );
    }
    public Tuple<List<OrderDetail>, long> GetOrderDetailsByID(int orderID, int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
    public Tuple<List<Order>, long> GetOrders(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
    public Tuple<List<Shoes>, long> GetShoes(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions)
    {
        //var shoesFields = new string[]{
        //    "ID", "Name", "Size", "Color"
        //};
        ////var shoesFields = new string[]{
        ////    "ID", "CategoryID", "Name", "Size", "Color", "Price", "Stock"
        ////};
        
        //var sortString = GetSortString(shoesFields, sortOptions);
        //var whereString = GetWhereString(shoesFields, whereOptions);
        //var sqlQuery = $"""
        //    SELECT count(*) over() as Total,"ShoeID","CategoryID","Name","Size","Color","Price","Stock" 
        //    FROM "Shoes" {whereString}
        //    {sortString} 
        //    LIMIT @Take
        //    OFFSET @Skip
        //""";
        //var command = new NpgsqlCommand(sqlQuery, dbConnection);
        //command.Parameters.Add("@Skip", NpgsqlDbType.Integer)
        //    .Value = (page - 1) * rowsPerPage;
        //command.Parameters.Add("@Take", NpgsqlDbType.Integer)
        //    .Value = rowsPerPage;
        //var reader = command.ExecuteReader();
        long totalShoes = 0;
        var result = new List<Shoes>();
        //while (reader.Read())
        //{
        //    if (totalShoes == -1)
        //    {
        //        totalShoes = (int)reader["Total"];
        //    }

        //    var shoes = new Shoes();
        //    shoes.ID = (int)reader["ShoeID"];
        //    shoes.CategoryID = (int)reader["CategoryID"];
        //    shoes.Name = (string)reader["Name"];
        //    shoes.Size = (string)reader["Size"];
        //    shoes.Color = (string)reader["Color"];
        //    shoes.Price = (decimal)reader["Price"];
        //    shoes.Stock = (int)reader["Stock"];
        //    //shoes.Avatar = (string)reader["Avatar"];
        //    result.Add(shoes);
        //}
        //reader.Close();
        return new Tuple<List<Shoes>, long>(
            result, totalShoes
        );
    }
    public User GetUserByID(int userID)
    {
        var sqlQuery = $"""
            SELECT "UserID","Name","Email","PhoneNumber"
            FROM "User" 
            WHERE "UserID" = @id
            """;
        var command=new NpgsqlCommand(sqlQuery,dbConnection);
        command.Parameters.Add("@id", NpgsqlDbType.Integer)
            .Value = userID;
        var reader=command.ExecuteReader();
        reader.Read();
        var user = new User();
        user.ID = (int)reader["UserID"];
        user.Name = (string)reader["Name"];
        user.Email = (string)reader["Email"];
        //user.Password = (string)reader["Password"];
        user.PhoneNumber = (string)reader["PhoneNumber"];
        reader.Close();
        return user;
    }
    public Tuple<List<User>, long> GetUsers(int page, int rowsPerPage, Dictionary<string, string> whereOptions, Dictionary<string, IDao.SortType> sortOptions) => throw new NotImplementedException();
}
