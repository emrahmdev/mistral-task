namespace backend.Models
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }

    }
}
