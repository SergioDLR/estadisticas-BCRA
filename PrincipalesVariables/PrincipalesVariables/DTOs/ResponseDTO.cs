namespace PrincipalesVariables.DTOs;

public class ResponseDTO<T>
{
    public T data { get; set; }
    public ErrorDTO[] errors { get; set; } = [];
}

