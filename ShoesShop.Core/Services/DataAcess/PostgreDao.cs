﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using OfficeOpenXml;
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
            Database = postgres;
            User ID = root;
            Password = root;
        """;
        dbConnection = new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    public PostgreDao(string serverUrl, int port, string databaseName, string userId, string password)
    {
        var connectionConfig = $"""
            Host = {serverUrl};
            Port={port};
            Database = {databaseName};
            User ID = {userId};
            Password = {password};
        """;
        var dbConnection = new NpgsqlConnection(connectionConfig);
        dbConnection.Open();
    }

    ~PostgreDao()
    {
        dbConnection.Close();
    }

    private string CreateSetForUpdate(string[] fields, string[] values)
    {
        if (fields.Length != values.Length || fields.Length == 0)
        {
            return "";
        }
        var setString = "";
        for (int i = 0; i < fields.Length; i++)
        {
            if (i != 0)
            {
                setString += ", ";
            }
            setString += $"\"{fields[i]}\" = \'{values[i]}\'";
        }
        return setString;
    }
    private string CreateUpdateQuery(string tableName, string idColumnName, int id, string[] fields, string[] values)
    {
        var setString = CreateSetForUpdate(fields, values);
        var updateQuery = $"""
            UPDATE "{tableName}"
            SET {setString}
            WHERE "{idColumnName}"='{id}'
            """;
        return updateQuery;
    }
    private string CreateInsertFields(string[] fields)
    {
        //expect output:(f1,f2,f3)
        var insertedFields = "(";
        for (int i = 0; i < fields.Length; i++)
        {
            if (i != 0)
            {
                insertedFields += ", ";
            }
            insertedFields += $"\"{fields[i]}\"";
        }
        insertedFields += ")";
        return insertedFields;
    }

    private string CreateInsertedValues(string[] values, string[] valueTypes)
    {
        var insertedValues = "(";
        for (int i = 0; i < values.Length; i++)
        {
            if (i != 0)
            {
                insertedValues += ", ";
            }
            if (valueTypes[i] == "string")
            {
                insertedValues += $"\'{values[i]}\'";
            }
            else if (valueTypes[i] == "integer")
            {
                var temp = int.Parse(values[i]);
                insertedValues += $"{temp}";
            }
            else if (valueTypes[i] == "decimal")
            {
                var temp = decimal.Parse(values[i]);
                insertedValues += $"{temp}";
            }
        }
        insertedValues += ")";
        return insertedValues;
    }
    private string CreateInsertQuery(string tableName, string columnIDName, string[] fields, string[] values, string[] valueTypes)
    {
        if (!(fields.Length == values.Length && fields.Length == valueTypes.Length) || fields.Length <= 0)
        {
            return "";
        }
        var insertedFields = CreateInsertFields(fields);
        var insertedValues = CreateInsertedValues(values, valueTypes);
        var sqlQuery = $"""
            INSERT INTO "{tableName}" {insertedFields}
            VALUES {insertedValues} RETURNING "{columnIDName}";
            """;
        return sqlQuery;
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

    private string GetWhereConditionDateFields(string[] dateFields, Dictionary<string, Tuple<string, string>> dateOptions)
    {
        var result = "";
        var countNumberFields = 0;
        foreach (var item in dateOptions)
        {
            if (dateFields.Contains(item.Key))
            {
                if (countNumberFields > 0)
                {
                    result += " AND ";
                }
                result += $"\"{item.Key}\" BETWEEN \'{item.Value.Item1}\' AND \'{item.Value.Item2}\'";
                countNumberFields++;
            }
        }
        return result;
    }
    private string GetWhereConditionNumberFields(string[] numberFields, Dictionary<string, Tuple<decimal, decimal>> numberOptions)
    {
        string result = "";
        var countNumberFields = 0;
        foreach (var item in numberOptions)
        {
            if (numberFields.Contains(item.Key))
            {
                if (countNumberFields > 0)
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

    private List<string> AddWhereConditions(params string[] conditions)
    {
        var res = new List<string>();
        foreach (var condition in conditions)
        {
            if (condition != "")
            {
                res.Add(condition);
            }
        }
        return res;
    }

    private string GetWhereCondition(
        string[] dateFields, Dictionary<string, Tuple<string, string>> dateOptions,
        string[] numberFields, Dictionary<string, Tuple<decimal, decimal>> numberOptions,
        string[] textFields, Dictionary<string, string> textOptions)
    {
        var res = "";
        var dateFieldsCondition = GetWhereConditionDateFields(dateFields, dateOptions);
        var textFieldsCondition = GetWhereConditionTextFields(textFields, textOptions);
        var numberFieldsCondition = GetWhereConditionNumberFields(numberFields, numberOptions);
        var whereConditions = AddWhereConditions(dateFieldsCondition, textFieldsCondition, numberFieldsCondition);
        var countCondition = 0;
        foreach (var condition in whereConditions)
        {
            if (countCondition == 0)
            {
                res += "WHERE ";
            }
            if (countCondition > 0)
            {
                res += " AND ";
            }
            res += condition;
            countCondition++;
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

    public Dictionary<int, string> GetAllCategories()
    {
        var categories = new Dictionary<int, string>();
        var sqlQuery = "SELECT \"CategoryID\", \"Name\" FROM \"Category\"";

        using (var command = new NpgsqlCommand(sqlQuery, dbConnection))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int categoryId = (int)reader["CategoryID"];
                    string categoryName = (string)reader["Name"];
                    categories.Add(categoryId, categoryName);
                }
                reader.Close();
            }
        }
        return categories;
    }


    public Tuple<List<Category>, long> GetCategories(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
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
        //tech-debt: re-factory
        var emptyfields = new string[0] { };
        var noUsage = new Dictionary<string, Tuple<string, string>>();
        //
        var whereString = GetWhereCondition(emptyfields, noUsage, categoryNumberFields, numberFieldsOptions, categoryTextFields, textFieldsOptions);
        //var selectFieldsString = GetSelectFields(categoryFields);
        var sqlQuery = $"""
            SELECT count(*) over() as Total, "CategoryID","Name","Description"
            FROM "Category" {whereString} {sortString}
            LIMIT @Take 
            OFFSET @Skip
            """;
        using (var command = new NpgsqlCommand(sqlQuery, dbConnection))
        {
            command.Parameters.Add("@Skip", NpgsqlDbType.Integer).Value = (page - 1) * rowsPerPage;
            command.Parameters.Add("@Take", NpgsqlDbType.Integer).Value = rowsPerPage;

            using (var reader = command.ExecuteReader())
            {
                long totalCategories = 0;
                while (reader.Read())
                {
                    if (totalCategories == 0)
                    {
                        totalCategories = (long)reader["Total"];
                    }
                    var category = new Category
                    {
                        ID = (int)reader["CategoryID"],
                        Name = (string)reader["Name"],
                        Description = (string)reader["Description"]
                    };
                    result.Add(category);
                }
                reader.Close();
                return new Tuple<List<Category>, long>(result, totalCategories);
            }
        }
    }


    public Tuple<Category, string, int> AddCategory(Category newCategory)
    {
        try
        {
            if (newCategory == null)
            {
                return new Tuple<Category, string, int>(null, "Can't add null Category", 1);
            }

            var fields = new string[] { "Name", "Description" };
            var values = new string[] { $"{newCategory.Name}", $"{newCategory.Description}" };
            var types = new string[] { "string", "string" };

            var query = CreateInsertQuery("Category", "CategoryID", fields, values, types);

            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                var id = command.ExecuteScalar();
                newCategory.ID = (int)id;
            }
            return new Tuple<Category, string, int>(newCategory, "success", 1);
        }
        catch (Exception e)
        {
            return new Tuple<Category, string, int>(null, e.Message, 0);
        }
    }


    public Tuple<Category, string, int> UpdateCategory(Category newCategory)
    {
        try
        {
            if (newCategory == null)
            {
                return new Tuple<Category, string, int>(null, "Can't update null Category", 0);
            }

            var fields = new string[] { "Name", "Description" };
            var values = new string[] { $"{newCategory.Name}", $"{newCategory.Description}" };
            var query = CreateUpdateQuery("Category", "CategoryID", newCategory.ID, fields, values);

            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                using (var reader = command.ExecuteReader())
                {

                }
            }

            var msg = "";
            return new Tuple<Category, string, int>(newCategory, msg, 1);
        }
        catch (Exception e)
        {
            return new Tuple<Category, string, int>(null, e.Message, 0);
        }
    }

    public Tuple<string, int> DeleteCategory(int categoryId)
    {
        try
        {
            if (categoryId <= 0)
            {
                return new Tuple<string, int>("Invalid Category ID", 0);
            }

            // Check if there are any shoes associated with this category
            var checkShoesQuery = $"SELECT COUNT(*) FROM \"Shoes\" WHERE \"CategoryID\" = @CategoryId";
            using (var checkCommand = new NpgsqlCommand(checkShoesQuery, dbConnection))
            {
                checkCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                var shoesCount = (long)checkCommand.ExecuteScalar();

                if (shoesCount > 0)
                {
                    // Update shoes to have a category ID of 1
                    var updateShoesQuery = $"UPDATE \"Shoes\" SET \"CategoryID\" = 1 WHERE \"CategoryID\" = @CategoryId";
                    using (var updateCommand = new NpgsqlCommand(updateShoesQuery, dbConnection))
                    {
                        updateCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }

            // Delete the category
            var deleteQuery = $"DELETE FROM \"Category\" WHERE \"CategoryID\" = @CategoryId";
            using (var deleteCommand = new NpgsqlCommand(deleteQuery, dbConnection))
            {
                deleteCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                var rowsAffected = deleteCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return new Tuple<string, int>("Category deleted successfully", rowsAffected);
                }
                else
                {
                    return new Tuple<string, int>("Category not found", 0);
                }
            }
        }
        catch (Exception e)
        {
            return new Tuple<string, int>(e.Message, 0);
        }
    }


    public Tuple<List<Shoes>, long> GetShoes(int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions)
    {
        try
        {
            var shoesTextFields = new string[]
            {
            "Name", "Brand", "Size", "Color", "Image", "Description", "Status"
            };
            var shoesNumberFields = new string[]
            {
            "ID", "CategoryID", "Cost", "Price", "Stock"
            };
            var shoesFields = shoesTextFields.Concat(shoesNumberFields).ToArray();
            var sortString = GetSortString(shoesFields, sortOptions);
            // tech-debt: re-factory
            var emptyfields = new string[0] { };
            var noUsage = new Dictionary<string, Tuple<string, string>>();
            //
            var whereString = GetWhereCondition(emptyfields, noUsage, shoesNumberFields,
                numberFieldsOptions, shoesTextFields, textFieldsOptions);
            var sqlQuery = $"""
            SELECT count(*) over() as Total, "ShoesID", "CategoryID", "Name", "Brand", "Size", "Color", "Cost", "Price", "Stock", "Image", "Description", "Status"
            FROM "Shoes" {whereString} {sortString} 
            LIMIT @Take
            OFFSET @Skip
        """;
            using (var command = new NpgsqlCommand(sqlQuery, dbConnection))
            {
                command.Parameters.Add("@Skip", NpgsqlDbType.Integer).Value = (page - 1) * rowsPerPage;
                command.Parameters.Add("@Take", NpgsqlDbType.Integer).Value = rowsPerPage;

                using (var reader = command.ExecuteReader())
                {
                    long totalShoes = 0;
                    var result = new List<Shoes>();
                    while (reader.Read())
                    {
                        if (totalShoes == 0)
                        {
                            totalShoes = (long)reader["Total"];
                        }

                        var shoes = new Shoes
                        {
                            ID = (int)reader["ShoesID"],
                            CategoryID = (int)reader["CategoryID"],
                            Name = (string)reader["Name"],
                            Brand = (string)reader["Brand"],
                            Size = (string)reader["Size"],
                            Color = (string)reader["Color"],
                            Cost = (decimal)reader["Cost"],
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            Image = reader["Image"] as string,
                            Description = (string)reader["Description"],
                            Status = (string)reader["Status"],
                        };
                        result.Add(shoes);
                    }
                    reader.Close();
                    return new Tuple<List<Shoes>, long>(result, totalShoes);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            return new Tuple<List<Shoes>, long>(new List<Shoes>(), 0);
        }
    }


    public Tuple<bool, string, Shoes> AddShoes(Shoes newShoes)
    {
        try
        {
            if (newShoes == null)
            {
                return new Tuple<bool, string, Shoes>(false, "Can't add null Shoes", null);
            }

            var fields = new string[] { "CategoryID", "Name", "Brand",
                "Size", "Color", "Cost", "Price", "Stock", "Image", "Description", "Status" };
            var values = new string[] { $"{newShoes.CategoryID}", $"{newShoes.Name}", $"{newShoes.Brand}", $"{newShoes.Size}",
                $"{newShoes.Color}", $"{newShoes.Cost}", $"{newShoes.Price}", $"{newShoes.Stock}", $"{newShoes.Image}", $"{newShoes.Description}", $"{newShoes.Status}" };
            var types = new string[] { "integer", "string", "string",
                "string", "string", "decimal", "decimal", "integer", "string", "string", "string" };
            var query = CreateInsertQuery("Shoes", "ShoesID", fields, values, types);

            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                var id = command.ExecuteScalar();
                newShoes.ID = Convert.ToInt32(id);
            }

            return new Tuple<bool, string, Shoes>(true, string.Empty, newShoes);
        }
        catch (Exception e)
        {
            return new Tuple<bool, string, Shoes>(false, e.Message, null);
        }
    }

    public Tuple<bool, string, Shoes> UpdateShoes(Shoes newShoes)
    {
        try
        {
            if (newShoes == null)
            {
                return new Tuple<bool, string, Shoes>(false, "Can't add null Shoes", null);
            }

            var fields = new string[] { "CategoryID", "Name", "Brand", "Size", "Color", "Cost", "Price", "Stock", "Image", "Description", "Status"  };
            var values = new string[] { $"{newShoes.CategoryID}", $"{newShoes.Name}", $"{newShoes.Brand}", $"{newShoes.Size}", $"{newShoes.Color}", $"{newShoes.Cost}", $"{newShoes.Price}", $"{newShoes.Stock}", $"{newShoes.Image}", $"{newShoes.Description}", $"{newShoes.Status}" };
            var query = CreateUpdateQuery("Shoes", "ShoesID", newShoes.ID, fields, values);

            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                command.ExecuteNonQuery();
            }

            return new Tuple<bool, string, Shoes>(true, string.Empty, newShoes);
        }
        catch (Exception e)
        {
            return new Tuple<bool, string, Shoes>(false, e.Message, null);
        }
    }


    
    public Tuple<bool, string> DeleteShoesByID(int shoesID)
    {
        try
        {
            bool result = false;
            var sqlQuery = $"""
            UPDATE "Shoes" 
            SET "Status" = CASE 
                WHEN "Status" = 'Active' THEN 'Inactive' 
                ELSE 'Active' 
            END
            WHERE "ShoesID"=@id
            """;
            using var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.Add("@id", NpgsqlDbType.Integer)
                .Value = shoesID;
            var row = command.ExecuteNonQuery();
            if (row != -1)
            {
                result = true;
            }
            var msg = result ? "Status toggled successfully" : "Can't find shoes with given ID";
            return new Tuple<bool, string>(result, msg);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }
    }



    public Tuple<bool, string, List<Detail>> GetDetailsByOrderIds(List<int> orderIds)
    {
        try
        {
            if (!orderIds.Any())
                return new Tuple<bool, string, List<Detail>>(true, string.Empty, new List<Detail>());

            var sqlQuery = $"""
        SELECT 
            d."DetailID", 
            d."OrderID", 
            d."ShoesID", 
            d."Quantity",
            d."Price",
            s."ShoesID",
            s."CategoryID", 
            s."Name", 
            s."Brand", 
            s."Size", 
            s."Color",
            s."Cost",
            s."Price" AS ShoesPrice, 
            s."Stock", 
            s."Image", 
            s."Description",
            s."Status"
        FROM "Detail" d
        INNER JOIN "Shoes" s ON d."ShoesID" = s."ShoesID"
        WHERE d."OrderID" = ANY(@OrderIds);
        """;

            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.AddWithValue("@OrderIds", orderIds);

            var details = new List<Detail>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var shoes = new Shoes
                    {
                        ID = (int)reader["ShoesID"],
                        CategoryID = (int)reader["CategoryID"],
                        Name = reader["Name"].ToString(),
                        Brand = reader["Brand"].ToString(),
                        Size = reader["Size"].ToString(),
                        Color = reader["Color"].ToString(),
                        Cost = (decimal)reader["Cost"],
                        Price = (decimal)reader["ShoesPrice"],
                        Stock = (int)reader["Stock"],
                        Image = reader["Image"].ToString(),
                        Description = reader["Description"].ToString(),
                        Status = reader["Status"].ToString()
                    };

                    var detail = new Detail
                    {
                        ID = (int)reader["DetailID"],
                        OrderID = (int)reader["OrderID"],
                        ShoesID = (int)reader["ShoesID"],
                        Quantity = (int)reader["Quantity"],
                        Price = (decimal)reader["Price"],
                        Shoes = shoes
                    };
                    details.Add(detail);
                }
            }

            return new Tuple<bool, string, List<Detail>>(true, string.Empty, details);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, List<Detail>>(false, ex.Message, null);
        }
    }




    public Tuple<bool, string, Dictionary<int, Shoes>> GetShoesByIds(List<int> shoesIds)
    {
        try
        {
            if (!shoesIds.Any())
                return new Tuple<bool, string, Dictionary<int, Shoes>>(true, string.Empty, new Dictionary<int, Shoes>());

            var sqlQuery = $"""
            SELECT "ShoesID", "CategoryID", "Name", "Brand", "Size", "Color", "Cost", "Price", "Stock", "Image", "Description", "Status"
            FROM "Shoes"
            WHERE "ShoesID" = ANY(@ShoesIDs);
            """;

            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.AddWithValue("@ShoesIds", shoesIds);

            var shoesDictionary = new Dictionary<int, Shoes>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var shoes = new Shoes
                    {
                        ID = (int)reader["ShoesID"],
                        CategoryID = (int)reader["CategoryID"],
                        Name = (string)reader["Name"],
                        Brand = (string)reader["Brand"],
                        Size = (string)reader["Size"],
                        Color = (string)reader["Color"],
                        Cost = (decimal)reader["Cost"],
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        Image = (string)reader["Image"],
                        Description = (string)reader["Description"],
                        Status = (string)reader["Status"]
                    };
                    shoesDictionary[shoes.ID] = shoes;
                }
            }

            return new Tuple<bool, string, Dictionary<int, Shoes>>(true, string.Empty, shoesDictionary);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, Dictionary<int, Shoes>>(false, ex.Message, null);
        }
    }


    public int GetTotalShoesSoldByIdAsync(int shoesId)
    {
        string query = @"
        SELECT COALESCE(SUM(""Quantity""), 0) AS TotalSold
        FROM ""Detail""
        WHERE ""ShoesID"" = @ShoesId;
    ";

        try
        {
            // Tạo và thiết lập NpgsqlCommand
            using var command = new NpgsqlCommand(query, dbConnection);
            command.Parameters.AddWithValue("@ShoesId", shoesId);

            // Thực thi câu truy vấn và trả về kết quả
            return (int)(Convert.ToInt64(command.ExecuteScalar() ?? 0));
        }
        catch (Exception ex)
        {
            // Ghi log hoặc xử lý lỗi cụ thể
            Console.Error.WriteLine($"Error fetching total shoes sold for ShoesID {shoesId}: {ex.Message}");
            throw; // Ném lại exception sau khi ghi log
        }
    }



    public Tuple<bool, string, Dictionary<int, User>> GetUserByIds(List<int> userIds)
    {
        try
        {
            if (!userIds.Any())
                return new Tuple<bool, string, Dictionary<int, User>>(true, string.Empty, new Dictionary<int, User>());

            var sqlQuery = $"""
            SELECT "UserID", "AddressID", "Name", "Email", "Password", "PhoneNumber", "Role", "Status", "Image"
            FROM "User"
            WHERE "UserID" = ANY(@UserIds);
            """;

            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.AddWithValue("@UserIds", userIds);

            var userDictionary = new Dictionary<int, User>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var user = new User
                    {
                        ID = (int)reader["UserID"],
                        AddressID = (int)reader["AddressID"],
                        Name = (string)reader["Name"],
                        Email = (string)reader["Email"],
                        Password = (string)reader["Password"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        Role = (string)reader["Role"],
                        Status = (string)reader["Status"],
                        Image = (string)reader["Image"]
                    };
                    userDictionary[user.ID] = user;
                }
            }

            return new Tuple<bool, string, Dictionary<int, User>>(true, string.Empty, userDictionary);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, Dictionary<int, User>>(false, ex.Message, null);
        }
    }


   
    public Tuple<bool, string, Dictionary<int, Address>> GetAddressesByIds(List<int> addressIds)
    {
        try
        {
            if (!addressIds.Any())
                return new Tuple<bool, string, Dictionary<int, Address>>(true, string.Empty, new Dictionary<int, Address>());

            var sqlQuery = $"""
            SELECT "AddressID", "Street", "City", "State", "ZipCode", "Country"
            FROM "Address"
            WHERE "AddressID" = ANY(@AddressIds);
            """;

            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.AddWithValue("@AddressIds", addressIds);

            var addressDictionary = new Dictionary<int, Address>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var address = new Address
                    {
                        ID = (int)reader["AddressID"],
                        Street = (string)reader["Street"],
                        City = (string)reader["City"],
                        State = (string)reader["State"],
                        ZipCode = (string)reader["ZipCode"],
                        Country = (string)reader["Country"]
                    };
                    addressDictionary[address.ID] = address;
                }
            }

            return new Tuple<bool, string, Dictionary<int, Address>>(true, string.Empty, addressDictionary);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, Dictionary<int, Address>>(false, ex.Message, null);
        }
    }

   

    public Tuple<bool, string, List<Order>, long> GetOrders(
        int page,
        int rowsPerPage,
        Dictionary<string, Tuple<string, string>> dateFieldsOptions,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions)
    {
        try
        {
            var orderTextFields = new string[] { "Status" };
            var orderNumberFields = new string[] { "OrderID", "UserID", "AddressID", "TotalAmount" };
            var orderDateFields = new string[] { "OrderDate" };
            var categoryFields = orderTextFields.Concat(orderNumberFields).ToArray();

            var sortString = GetSortString(categoryFields, sortOptions);
            var whereString = GetWhereCondition(orderDateFields, dateFieldsOptions, orderNumberFields, numberFieldsOptions, orderTextFields, textFieldsOptions);

            // SQL query to get Orders
            var sqlQueryOrders = $"""
            SELECT count(*) over() as Total, "OrderID", "UserID", "OrderDate", "Status", "AddressID", "TotalAmount"
            FROM "Order" {whereString} {sortString}
            LIMIT @Take 
            OFFSET @Skip;
            """;

            var commandOrders = new NpgsqlCommand(sqlQueryOrders, dbConnection);
            commandOrders.Parameters.Add("@Skip", NpgsqlDbType.Integer).Value = (page - 1) * rowsPerPage;
            commandOrders.Parameters.Add("@Take", NpgsqlDbType.Integer).Value = rowsPerPage;

            var orders = new List<Order>();
            long totalOrders = 0;

            using (var reader = commandOrders.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (totalOrders == 0)
                    {
                        totalOrders = (long)reader["Total"];
                    }

                    var order = new Order
                    {
                        ID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        Status = (string)reader["Status"],
                        OrderDate = ((DateTime)reader["OrderDate"]).ToString("yyyy-MM-dd"),
                        AddressID = (int)reader["AddressID"],
                        TotalAmount = (decimal)reader["TotalAmount"],
                        Details = new List<Detail>() // Details will be added later
                    };

                    orders.Add(order);
                }
            }

            if (orders.Count > 0)
            {
                // Get AddressIDs from Orders
                var addressIds = orders.Select(o => o.AddressID).Distinct().ToList();
                var (addressSuccess, addressMessage, addressDictionary) = GetAddressesByIds(addressIds);
                if (!addressSuccess)
                {
                    return new Tuple<bool, string, List<Order>, long>(false, addressMessage, null, 0);
                }

                foreach (var order in orders)
                {
                    if (addressDictionary.ContainsKey(order.AddressID))
                    {
                        order.Address = addressDictionary[order.AddressID];
                    }
                }

                // Get UserIDs from Orders
                var userIds = orders.Select(o => o.UserID).Distinct().ToList();
                var (userSuccess, userMessage, userDictionary) = GetUserByIds(userIds);
                if (!userSuccess)
                {
                    return new Tuple<bool, string, List<Order>, long>(false, userMessage, null, 0);
                }

                foreach (var order in orders)
                {
                    if (userDictionary.ContainsKey(order.UserID))
                    {
                        order.User = userDictionary[order.UserID];
                    }
                }

                // Get Details
                var orderIds = orders.Select(o => o.ID).ToList();
                var (detailSuccess, detailMessage, details) = GetDetailsByOrderIds(orderIds);
                if (!detailSuccess)
                {
                    return new Tuple<bool, string, List<Order>, long>(false, detailMessage, null, 0);
                }

                foreach (var order in orders)
                {
                    order.Details = details.Where(d => d.OrderID == order.ID).ToList();
                }

                // Get Shoes
                var shoesIds = details.Select(d => d.ShoesID).Distinct().ToList();
                var (shoesSuccess, shoesMessage, shoesDictionary) = GetShoesByIds(shoesIds);
                if (!shoesSuccess)
                {
                    return new Tuple<bool, string, List<Order>, long>(false, shoesMessage, null, 0);
                }

                foreach (var detail in details)
                {
                    if (shoesDictionary.ContainsKey(detail.ShoesID))
                    {
                        detail.Shoes = shoesDictionary[detail.ShoesID];
                    }
                }
            }

            return new Tuple<bool, string, List<Order>, long>(true, "Success", orders, totalOrders);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, List<Order>, long>(false, ex.Message, null, 0);
        }
    }




    public Tuple<bool, string, Order> AddOrder(Order newOrder)
    {
        using (var transaction = dbConnection.BeginTransaction())
        {
            try
            {
                // 1. Thêm Order mới
                var insertOrderQuery = $"""
                INSERT INTO "Order" ("UserID", "OrderDate", "Status", "AddressID", "TotalAmount")
                VALUES (@UserID, @OrderDate, @Status, @AddressID, @TotalAmount)
                RETURNING "OrderID";
                """;

                var orderCommand = new NpgsqlCommand(insertOrderQuery, dbConnection);
                orderCommand.Transaction = transaction;

                // Gán tham số
                orderCommand.Parameters.AddWithValue("@UserID", newOrder.UserID);
                orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.UtcNow);
                orderCommand.Parameters.AddWithValue("@Status", newOrder.Status);
                orderCommand.Parameters.AddWithValue("@AddressID", 1);
                orderCommand.Parameters.AddWithValue("@TotalAmount", newOrder.Details.Sum(d => d.Price));

                var orderId = (int)orderCommand.ExecuteScalar();

                // 2. Thêm chi tiết OrderDetails
                foreach (var detail in newOrder.Details)
                {
                    var insertDetailQuery = $"""
                    INSERT INTO "Detail" ("OrderID", "ShoesID", "Quantity", "Price")
                    VALUES (@OrderID, @ShoesID, @Quantity, @Price);
                    """;

                    var detailCommand = new NpgsqlCommand(insertDetailQuery, dbConnection);
                    detailCommand.Transaction = transaction;

                    detailCommand.Parameters.AddWithValue("@OrderID", orderId);
                    detailCommand.Parameters.AddWithValue("@ShoesID", detail.ShoesID);
                    detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                    detailCommand.Parameters.AddWithValue("@Price", detail.Price);

                    detailCommand.ExecuteScalar();
                }

                // 3. Commit giao dịch
                transaction.Commit();

                // 4. Tạo đối tượng Order để trả về
                var createdOrder = new Order
                {
                    ID = orderId,
                    UserID = newOrder.UserID,
                    OrderDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    Status = "Pending",
                    TotalAmount = newOrder.Details.Sum(d => d.Price)
                };

                return Tuple.Create(true, "Order created successfully.", createdOrder);
            }
            catch (Exception ex)
            {
                // Rollback giao dịch nếu có lỗi
                transaction.Rollback();
                return Tuple.Create(false, $"Failed to create order: {ex.Message}", (Order)null);
            }
        }
    }


    public Tuple<bool, string> AddDetail(int orderId, Detail detail)
    {
        try
        {
            // Câu lệnh thêm Detail
            var insertDetailQuery = $"""
            INSERT INTO "Detail" ("OrderID", "ShoesID", "Quantity", "Price")
            VALUES (@OrderID, @ShoesID, @Quantity, @Price);
            """;

            using (var command = new NpgsqlCommand(insertDetailQuery, dbConnection))
            {
                command.Parameters.AddWithValue("@OrderID", orderId);
                command.Parameters.AddWithValue("@ShoesID", detail.ShoesID);
                command.Parameters.AddWithValue("@Quantity", detail.Quantity);
                command.Parameters.AddWithValue("@Price", detail.Price);

                command.ExecuteNonQuery();
                return Tuple.Create(true, "Detail added successfully.");
            }
        }
        catch (Exception ex)
        {
            return Tuple.Create(false, $"Failed to add detail: {ex.Message}");
        }
    }


    
    public Tuple<bool, string, Order> UpdateOrder(Order order)
    {
        using (var transaction = dbConnection.BeginTransaction())
        {
            try
            {
                if (order.Address != null)
                {
                    var upsertAddressQuery = $"""
                    INSERT INTO "Address" ("Street", "City", "State", "ZipCode", "Country")
                    VALUES (@Street, @City, @State, @ZipCode, @Country)
                    RETURNING "AddressID";
                    """;

                    var addressCommand = new NpgsqlCommand(upsertAddressQuery, dbConnection);
                    addressCommand.Transaction = transaction;


                    addressCommand.Parameters.AddWithValue("@Street", order.Address.Street);
                    addressCommand.Parameters.AddWithValue("@City", order.Address.City);
                    addressCommand.Parameters.AddWithValue("@State", order.Address.State);
                    addressCommand.Parameters.AddWithValue("@ZipCode", order.Address.ZipCode);
                    addressCommand.Parameters.AddWithValue("@Country", order.Address.Country);

                    order.AddressID = (int)addressCommand.ExecuteScalar();
                }
                //Cập nhật thông tin Order
                var updateOrderQuery = $"""
                UPDATE "Order"
                SET "UserID" = @UserID, 
                    "OrderDate" = @OrderDate,
                    "Status" = @Status,
                    "AddressID" = @AddressID,
                    "TotalAmount" = @TotalAmount
                WHERE "OrderID" = @OrderID;
                """;

                var orderCommand = new NpgsqlCommand(updateOrderQuery, dbConnection);
                orderCommand.Transaction = transaction;


                orderCommand.Parameters.AddWithValue("@OrderID", order.ID);
                orderCommand.Parameters.AddWithValue("@UserID", order.UserID);
                orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Parse(order.OrderDate));
                orderCommand.Parameters.AddWithValue("@Status", order.Status);
                orderCommand.Parameters.AddWithValue("@AddressID", order.AddressID);
                orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);

                orderCommand.ExecuteNonQuery();

                //Delete and insert new details
                if (order.Details != null && order.Details.Any())
                {
                    var deleteDetailsQuery = $"""
                    DELETE FROM "Detail"
                    WHERE "OrderID" = @OrderID;
                    """;

                    var deleteDetailsCommand = new NpgsqlCommand(deleteDetailsQuery, dbConnection);
                    deleteDetailsCommand.Transaction = transaction;
                    deleteDetailsCommand.Parameters.AddWithValue("@OrderID", order.ID);
                    deleteDetailsCommand.ExecuteNonQuery();

                    foreach (var detail in order.Details)
                    {
                        var insertDetailQuery = $"""
                        INSERT INTO "Detail" ("OrderID", "ShoesID", "Quantity", "Price")
                        VALUES (@OrderID, @ShoesID, @Quantity, @Price);
                        """;

                        var detailCommand = new NpgsqlCommand(insertDetailQuery, dbConnection);
                        detailCommand.Transaction = transaction;

                        detailCommand.Parameters.AddWithValue("@OrderID", order.ID);
                        detailCommand.Parameters.AddWithValue("@ShoesID", detail.ShoesID);
                        detailCommand.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        detailCommand.Parameters.AddWithValue("@Price", detail.Price);

                        detailCommand.ExecuteNonQuery();
                    }
                }

                transaction.Commit();

                return Tuple.Create(true, "Order updated successfully.", order);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Tuple.Create(false, $"Failed to update order: {ex.Message}", order);
            }
        }
    }



    public Tuple<bool, string> DeleteOrder(Order order)
    {
        using (var transaction = dbConnection.BeginTransaction())
        {
            try
            {
                // 1. Xóa OrderDetails liên quan
                var deleteDetailsQuery = $"""
                DELETE FROM "Detail"
                WHERE "OrderID" = @OrderID;
                """;

                var deleteDetailsCommand = new NpgsqlCommand(deleteDetailsQuery, dbConnection);
                deleteDetailsCommand.Transaction = transaction;
                deleteDetailsCommand.Parameters.AddWithValue("@OrderID", order.ID);
                deleteDetailsCommand.ExecuteNonQuery();

                // 2. Xóa Order
                var deleteOrderQuery = $"""
                DELETE FROM "Order"
                WHERE "OrderID" = @OrderID;
            """;

                var deleteOrderCommand = new NpgsqlCommand(deleteOrderQuery, dbConnection);
                deleteOrderCommand.Transaction = transaction;
                deleteOrderCommand.Parameters.AddWithValue("@OrderID", order.ID);
                deleteOrderCommand.ExecuteNonQuery();

                // 3. Commit giao dịch
                transaction.Commit();

                return Tuple.Create(true, "Order deleted successfully.");
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                transaction.Rollback();
                return Tuple.Create(false, $"Failed to delete order: {ex.Message}");
            }
        }
    }


    public Tuple<bool, string> DeleteDetail(int orderId, int detailId)
    {
        try
        {
            var deleteDetailQuery = $"""
            DELETE FROM "Detail"
            WHERE "OrderID" = @OrderID AND "DetailID" = @DetailID;
            """;

            using (var command = new NpgsqlCommand(deleteDetailQuery, dbConnection))
            {
                command.Parameters.AddWithValue("@OrderID", orderId);
                command.Parameters.AddWithValue("@DetailID", detailId);
                command.ExecuteNonQuery();
            }

            return Tuple.Create(true, "Detail deleted successfully.");
        }
        catch (Exception ex)
        {
            return Tuple.Create(false, $"Failed to delete detail: {ex.Message}");
        }
    }

    


    public Tuple<bool, string, Detail> AddDetail(Detail newDetail)
    {
        try
        {
            if (newDetail == null)
            {
                return new Tuple<bool, string, Detail>(false, "Can't add null Detail", null);
            }

            var fields = new string[] { "OrderID", "ShoesID", "Quantity", "Price" };
            var values = new string[] { $"{newDetail.OrderID}", $"{newDetail.ShoesID}", $"{newDetail.Quantity}", $"{newDetail.Price}" };
            var types = new string[] { "integer", "integer", "integer", "decimal" };
            var query = CreateInsertQuery("Details", "DetailID", fields, values, types);

            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                var id = command.ExecuteScalar();
                newDetail.ID = Convert.ToInt32(id);
            }

            return new Tuple<bool, string, Detail>(true, string.Empty, newDetail);
        }
        catch (Exception e)
        {
            return new Tuple<bool, string, Detail>(false, e.Message, null);
        }
    }


    public Tuple<bool, string, Address> AddAddress(Address address)
    {
        try
        {
            string query = $"""
            INSERT INTO "Address" ("Street", "City", "State", "ZipCode", "Country")
            VALUES (@Street, @City, @State, @ZipCode, @Country)
            RETURNING "AddressID";
            """;

            using var command = new NpgsqlCommand(query, dbConnection);
            command.Parameters.AddWithValue("@Street", address.Street);
            command.Parameters.AddWithValue("@City", address.City);
            command.Parameters.AddWithValue("@State", address.State);
            command.Parameters.AddWithValue("@ZipCode", address.ZipCode);
            command.Parameters.AddWithValue("@Country", address.Country);

            var newAddressId = (int)command.ExecuteScalar();
            address.ID = newAddressId;

            return new Tuple<bool, string, Address>(true, "Address added successfully", address);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, Address>(false, $"An error occurred: {ex.Message}", null);
        }
    }



    public User GetUserByID(int userID)
    {
        var sqlQuery = $"""
            SELECT "UserID","Name","Email","PhoneNumber"
            FROM "User" 
            WHERE "UserID" = @id
            """;
        var command = new NpgsqlCommand(sqlQuery, dbConnection);
        command.Parameters.Add("@id", NpgsqlDbType.Integer)
            .Value = userID;
        var reader = command.ExecuteReader();
        var user = new User();
        if (reader.Read())
        {
            user.ID = (int)reader["UserID"];
            user.Name = (string)reader["Name"];
            user.Email = (string)reader["Email"];
            user.PhoneNumber = (string)reader["PhoneNumber"];
        }
        reader.Close();
        return user;
    }
    public Tuple<bool, string, List<User>, long> GetUsers(
        int page, int rowsPerPage,
        Dictionary<string, Tuple<decimal, decimal>> numberFieldsOptions,
        Dictionary<string, string> textFieldsOptions,
        Dictionary<string, IDao.SortType> sortOptions)
    {
        try
        {
            var userTextFields = new string[]
            {
            "Name", "Email", "PhoneNumber", "Role", "Status", "Image"
            };
            var userNumberFields = new string[]
            {
            "UserID", "AddressID", 
            };
            var userFields = userTextFields.Concat(userNumberFields).ToArray();
            var sortString = GetSortString(userFields, sortOptions);

            // tech-debt: re-factory
            var emptyFields = new string[0];
            var noUsage = new Dictionary<string, Tuple<string, string>>();

            var whereString = GetWhereCondition(emptyFields, noUsage, userNumberFields, numberFieldsOptions, userTextFields, textFieldsOptions);

            var sqlQuery = $"""
            SELECT count(*) over() as Total, "UserID", "Name", "Password", "Email", "PhoneNumber", "AddressID", "Role", "Status", "Image"
            FROM "User" {whereString} {sortString}
            LIMIT @Take 
            OFFSET @Skip
            """;

            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            command.Parameters.Add("@Skip", NpgsqlDbType.Integer).Value = (page - 1) * rowsPerPage;
            command.Parameters.Add("@Take", NpgsqlDbType.Integer).Value = rowsPerPage;

            var users = new List<User>();
            long totalUsers = 0;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (totalUsers == 0)
                    {
                        totalUsers = (long)reader["Total"];
                    }

                    var user = new User
                    {
                        ID = (int)reader["UserID"],
                        Name = (string)reader["Name"],
                        Email = (string)reader["Email"],
                        Password = (string)reader["Password"],
                        PhoneNumber = (string)reader["PhoneNumber"],
                        AddressID = (int)reader["AddressID"],
                        Role = (string)reader["Role"],
                        Status = (string)reader["Status"],
                        Image = (string)reader["Image"]
                    };

                    users.Add(user);
                }
            }

            var addressIds = users.Select(u => u.AddressID).Distinct().ToList();
            var (addressSuccess, addressMessage, addressDictionary) = GetAddressesByIds(addressIds);
            if (!addressSuccess)
            {
                return new Tuple<bool, string, List<User>, long>(false, addressMessage, null, 0);
            }

            foreach (var user in users)
            {
                if (addressDictionary.ContainsKey(user.AddressID))
                {
                    user.Address = addressDictionary[user.AddressID];
                }
            }

            return new Tuple<bool, string, List<User>, long>(true, "Success", users, totalUsers);
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string, List<User>, long>(false, ex.Message, new List<User>(), 0);
        }
    }

    public Tuple<bool, string> BanAndUnbanUser(User user)
    {
        try
        {
            var sqlQuery = $"""
            UPDATE "User"
            SET "Status" = @Status
            WHERE "UserID" = @UserID;
            """;

            using (var command = new NpgsqlCommand(sqlQuery, dbConnection))
            {
                command.Parameters.AddWithValue("@UserID", user.ID);
                command.Parameters.AddWithValue("@Status", user.Status);

                var rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return new Tuple<bool, string>(true, "User banned successfully");
                }
                else
                {
                    return new Tuple<bool, string>(false, "User not found");
                }
            }
        }
        catch (Exception ex)
        {
            return new Tuple<bool, string>(false, ex.Message);
        }
    }

    public Tuple<bool, string, User> UpdateUser(User user)
    {
        using (var transaction = dbConnection.BeginTransaction())
        {
            try
            {
                // Update address
                var addressQuery = $"""
            UPDATE "Address"
            SET 
                "Street" = @Street,
                "City" = @City,
                "State" = @State,
                "ZipCode" = @ZipCode
            WHERE "AddressID" = @AddressID;
            """;

                using (var addressCommand = new NpgsqlCommand(addressQuery, dbConnection, transaction))
                {
                    addressCommand.Parameters.AddWithValue("@AddressID", user.AddressID);
                    addressCommand.Parameters.AddWithValue("@Street", user.Address.Street);
                    addressCommand.Parameters.AddWithValue("@City", user.Address.City);
                    addressCommand.Parameters.AddWithValue("@State", user.Address.State);
                    addressCommand.Parameters.AddWithValue("@ZipCode", user.Address.ZipCode);

                    addressCommand.ExecuteNonQuery();
                }

                // Update user
                var userQuery = $"""
            UPDATE "User"
            SET 
                "Name" = @Name,
                "Email" = @Email,
                "PhoneNumber" = @PhoneNumber,
                "Role" = @Role,
                "Status" = @Status,
                "Image" = @Image,
                "Password" = @Password
            WHERE "UserID" = @UserID;
            """;

                using (var userCommand = new NpgsqlCommand(userQuery, dbConnection, transaction))
                {
                    userCommand.Parameters.AddWithValue("@UserID", user.ID);
                    userCommand.Parameters.AddWithValue("@Name", user.Name);
                    userCommand.Parameters.AddWithValue("@Email", user.Email);
                    userCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    userCommand.Parameters.AddWithValue("@Role", user.Role);
                    userCommand.Parameters.AddWithValue("@Status", user.Status);
                    userCommand.Parameters.AddWithValue("@Image", user.Image ?? (object)DBNull.Value);
                    userCommand.Parameters.AddWithValue("@Password", user.Password);

                    userCommand.ExecuteNonQuery();
                }

                // Commit transaction
                transaction.Commit();
                return new Tuple<bool, string, User>(true, "Update successful", user);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new Tuple<bool, string, User>(false, ex.Message, null);
            }
        }
    }


    public Tuple<bool, string, User> AddUser(User user)
    {
        using var transaction = dbConnection.BeginTransaction();
        try
        {
            // Nếu Address không null, thêm địa chỉ trước
            if (user.Address != null)
            {
                var addressQuery = $"""
                INSERT INTO "Address" ("Street", "City", "State", "ZipCode", "Country")
                VALUES (@Street, @City, @State, @ZipCode, @Country)
                RETURNING "AddressID";
                """;

                using (var addressCommand = new NpgsqlCommand(addressQuery, dbConnection, transaction))
                {
                    addressCommand.Parameters.AddWithValue("@Street", user.Address.Street);
                    addressCommand.Parameters.AddWithValue("@City", user.Address.City);
                    addressCommand.Parameters.AddWithValue("@State", user.Address.State);
                    addressCommand.Parameters.AddWithValue("@ZipCode", user.Address.ZipCode);
                    addressCommand.Parameters.AddWithValue("@Country", user.Address.Country); // Add this line

                    user.AddressID = (int)addressCommand.ExecuteScalar();
                }

            }

            // Thêm thông tin User
            var userQuery = $"""
            INSERT INTO "User" 
                ("Name", "Email", "PhoneNumber", "Role", "Status", "Image", "Password", "AddressID")
            VALUES 
                (@Name, @Email, @PhoneNumber, @Role, @Status, @Image, @Password, @AddressID)
            RETURNING "UserID", "Name", "Email", "PhoneNumber", "Role", "Status", "Image", "Password", "AddressID";
            """;

            using (var userCommand = new NpgsqlCommand(userQuery, dbConnection, transaction))
            {
                userCommand.Parameters.AddWithValue("@Name", user.Name);
                userCommand.Parameters.AddWithValue("@Email", user.Email);
                userCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                userCommand.Parameters.AddWithValue("@Role", user.Role);
                userCommand.Parameters.AddWithValue("@Status", user.Status);
                userCommand.Parameters.AddWithValue("@Image", user.Image ?? (object)DBNull.Value);
                userCommand.Parameters.AddWithValue("@Password", user.Password);
                userCommand.Parameters.AddWithValue("@AddressID", user.Address != null ? user.AddressID : (object)DBNull.Value);

                userCommand.ExecuteNonQuery();
            }

            transaction.Commit();
            return new Tuple<bool, string, User>(true, "User added successfully", user);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new Tuple<bool, string, User>(false, ex.Message, null);
        }
    }

    public Tuple<bool, string, List<User>> GetBannedUsers()
    {
        List<User> bannedUsers = new List<User>();
        string query = $"""
        SELECT * 
        FROM "User"
        WHERE "Status" = 'Banned'
        """;
        try
        {
            using (var cmd = new NpgsqlCommand(query, dbConnection))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            // Assuming User class has these properties
                            ID = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            // Add other properties as needed
                        };
                        bannedUsers.Add(user);
                    }
                }
            }
            return Tuple.Create(true, string.Empty, bannedUsers);
        }
        catch (Exception ex)
        {
            return Tuple.Create(false, ex.Message, bannedUsers);
        }
    }





    // dashboard 

    public async Task<int> GetTotalOrdersAsync()
    {
        long totalOrders = 0;
        string query = "SELECT COUNT(*) FROM \"Order\"";

        await using (var command = new NpgsqlCommand(query, dbConnection))
        {
            totalOrders = (long)await command.ExecuteScalarAsync();
        }

        

        return (int)totalOrders;
    }


    public async Task<List<Order>> GetRecentOrdersAsync()
    {
        List<Order> recentOrders = new List<Order>();
        string query = "SELECT * FROM \"Order\" ORDER BY \"OrderDate\" DESC LIMIT 5";

        using (NpgsqlCommand cmd = new NpgsqlCommand(query, dbConnection))
        {
            using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Order order = new Order
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("OrderID")),
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        AddressID = reader.GetInt32(reader.GetOrdinal("AddressID")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")).ToString("yyyy-MM-dd"),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                    };
                    recentOrders.Add(order);
                }
            }
           
        }

        return recentOrders;
    }

    public async Task<int> GetTotalRevenueAsync()
    {

        try
        {
            string query = @"
                SELECT SUM(o.""TotalAmount"") 
                FROM ""Order"" o
                WHERE o.""Status"" = 'Delivered'";

            await using (var command = new NpgsqlCommand(query, dbConnection))
            {
                var result = await command.ExecuteScalarAsync();
                if (result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error calculating total revenue", ex);
        }

        return 0;
    }

    public async Task<List<Shoes>> GetTop5BestSellingShoesAsync()
    {
        var topSellingShoes = new List<Shoes>();

        try
        {

            var query = @"
                SELECT s.*, SUM(d.""Quantity"") as ""TotalSold""
                FROM ""Order"" o
                JOIN ""Detail"" d ON o.""OrderID"" = d.""OrderID""
                JOIN ""Shoes"" s ON d.""ShoesID"" = s.""ShoesID""
                GROUP BY s.""ShoesID""
                ORDER BY ""TotalSold"" DESC
                LIMIT 5";


            using (var command = new NpgsqlCommand(query, dbConnection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var shoes = new Shoes
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ShoesID")),
                            CategoryID = reader.GetInt32(reader.GetOrdinal("CategoryID")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Size = reader.GetString(reader.GetOrdinal("Size")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                            Image = reader.GetString(reader.GetOrdinal("Image")),
                            Description = reader.GetString(reader.GetOrdinal("Description"))
                        };
                        topSellingShoes.Add(shoes);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error retrieving top 5 best-selling shoes", ex);
        }
        

        return topSellingShoes;
    }

    public async Task<int> GetTotalShoesInStockAsync()
    {
        var query = @"
        SELECT SUM(""Stock"") as ""TotalStock""
        FROM ""Shoes""";

        try
        {
            await using var command = new NpgsqlCommand(query, dbConnection);
            var result = await command.ExecuteScalarAsync();

            if (result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving total shoes in stock: {ex.Message}");
            throw new Exception("Error retrieving total shoes in stock", ex);
        }
    }

    public async Task<Dictionary<string, int>> GetOrderStatisticsAsync(string groupBy)
    {
        var statistics = new Dictionary<string, int>();

        string query = groupBy.ToLower() switch
        {
            "month" => @"SELECT TO_CHAR(""OrderDate"", 'YYYY-MM') AS period, COUNT(*) AS count FROM ""Order"" GROUP BY period ORDER BY period",
            "year" => @"SELECT TO_CHAR(""OrderDate"", 'YYYY') AS period, COUNT(*) AS count FROM ""Order"" GROUP BY period ORDER BY period",
            _ => throw new ArgumentException("Invalid groupBy value. Use 'month' or 'year'.")
        };

        await using var cmd = new NpgsqlCommand(query, dbConnection);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string period = reader.GetString(0);
            int count = reader.GetInt32(1);
            statistics[period] = count;
        }

        return statistics;
    }

    public User GetUserByName(string username)
    {
        try
        {
            var sqlQuery = $"""
            SELECT "UserID","Name","Email","Password","PhoneNumber","AddressID","Role","Status"
            FROM "User" 
            WHERE "Email" ='{username}'
            """;
            var command = new NpgsqlCommand(sqlQuery, dbConnection);
            //command.Parameters.Add("@username", NpgsqlDbType.Text)
            //    .Value = username;
            var reader = command.ExecuteReader();
            var user = new User();
            if (reader.Read())
            {
                user.ID = (int)reader["UserID"];
                user.Name = (string)reader["Name"];
                user.Email = (string)reader["Email"];
                user.Password = (string)reader["Password"];
                user.PhoneNumber = (string)reader["PhoneNumber"];
                user.AddressID = (int)reader["AddressID"];
                user.Role = (string)reader["Role"];
                user.Status = (string)reader["Status"];
            }
            reader.Close();
            return user;
        }
        catch(Exception e)
        {
            return null;
        }
    }



}
