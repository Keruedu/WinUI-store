using System.Text.Json.Serialization;

namespace ShoesShop.Core.Http;


public class Error
{
    [JsonPropertyName("errorMessage")]
    public string Message
    {

        get; set;
    }

    [JsonPropertyName("errorCode")]
    public string Code
    {
        get; set;
    }
}

public class HttpDataSchemaResponse<T>
{
    [JsonPropertyName("data")]
    public T Data
    {
        get; set;
    }

    [JsonPropertyName("error")]
    public Error Error
    {
        get; set;
    }

    [JsonPropertyName("message")]
    public string Message
    {
        get; set;
    }
}