
namespace Backup
{
    public class ServiceResponse<T>
    {
        public bool isSuccess { get; set; }

        public T? data { get; set; }

        public string? message { get; set; }
    }
}
