namespace kumablogB.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; } = true;
        public string? Error { get; set; } = null;
        public T? Data { get; set; }
    }
}
