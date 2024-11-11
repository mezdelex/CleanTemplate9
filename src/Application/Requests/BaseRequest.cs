namespace Application.Requests;

public record BaseRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
