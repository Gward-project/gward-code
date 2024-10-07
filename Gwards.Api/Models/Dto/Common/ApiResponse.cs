namespace Gwards.Api.Models.Dto.Responses;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Error { get; set; }
}
